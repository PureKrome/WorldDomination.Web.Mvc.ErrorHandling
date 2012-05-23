using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace WorldDomination.Web.Mvc
{
    public sealed class CustomErrorHandlingModule : IHttpModule
    {
        #region Implementation of IHttpModule

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="httpApplication">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication httpApplication)
        {
            //const string errorPage = "~/Views/Shared/Error.cshtml";

            httpApplication.Error += (sender, e) =>
                                     {
                                         if (!httpApplication.Context.IsCustomErrorEnabled)
                                         {
                                             // Damn it :( Fine.... lets just bounce outta-here!
                                             return;
                                         }

                                         // By default, this is a code (server) 500 error.
                                         // If we figure out that it's something else like a 404 or 401, etc
                                         // then we'll handle that next.
                                         HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

                                         // Lets remember the current error.
                                         Exception currentError = HttpContext.Current.Error;

                                         // Do we have an HttpErrorException? Eg. 404 Not found or 401 Not Authorised?
                                         var httpErrorException = currentError as HttpException;
                                         if (httpErrorException != null)
                                         {
                                             httpStatusCode = (HttpStatusCode) httpErrorException.GetHttpCode();
                                         }

                                         // What is the view we require?
                                         string viewPath = GetCustomErrorRedirect(httpStatusCode);
                                         if (string.IsNullOrEmpty(viewPath))
                                         {
                                             // Either
                                             // 1. No customErrors was provided (which would be weird cause how did we get here?)
                                             // 2. No redirect was provided for the httpStatus code.
                                         }

                                         // Lets clear all the errors otherwise it shows the error page.
                                         HttpContext.Current.ClearError();

                                         // Now - what do we render? The view provided or some basic content becuase one HASN'T been provided?
                                         if (string.IsNullOrEmpty(viewPath))
                                         {
                                             RenderFallBackErrorViewBecauseNoneWasProvided(httpApplication, currentError);
                                         }
                                         else
                                         {
                                             RenderCustomErrorView(httpApplication, viewPath, currentError);
                                         }

                                         // Lets make sure we set the correct Error Status code :)
                                         httpApplication.Response.StatusCode = (int) httpStatusCode;

                                         // Avoid any IIS low lever errors.
                                         httpApplication.Response.TrySkipIisCustomErrors = true;
                                     };
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            // No Op.
        }

        #endregion

        private static string GetCustomErrorRedirect(HttpStatusCode httpStatusCode)
        {
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("/");
            var section = configuration.GetSection("system.web/customErrors") as CustomErrorsSection;

            if (section == null)
            {
                return null;
            }

            string redirect = null;
            if (section.Errors != null)
            {
                CustomError customError = section.Errors[((int)httpStatusCode).ToString()];
                if (customError != null)
                {
                    redirect = customError.Redirect;
                }
            }

            // Have we a redirect, yet? If not, then use the default if we have that.
            return string.IsNullOrEmpty(redirect) ? section.DefaultRedirect : redirect;
        }

        private static void RenderCustomErrorView(HttpApplication httpApplication, string viewPath,
                                                  Exception currentError)
        {
            try
            {
                // We need to render the view now.
                // This means we need a viewContext ... which requires a controller.
                // So we instantiate a fake controller which does nothing
                // and then work our way to rendering the view.
                var errorController = new FakeErrorController();
                var controllerContext =
                    new ControllerContext(httpApplication.Context.Request.RequestContext, errorController);
                var view = new RazorView(controllerContext, viewPath, null, false, null);
                var viewModel = new ErrorViewModel
                                {
                                    Exception = currentError
                                };
                var tempData = new TempDataDictionary();
                var viewContext = new ViewContext(controllerContext, view,
                                                  new ViewDataDictionary(viewModel), tempData,
                                                  httpApplication.Response.Output);
                view.Render(viewContext, httpApplication.Response.Output);
            }
            catch(Exception exception)
            {
                // Damn it! Something -really- crap just happened. 
                // eg. the path to the redirect might not exist, etc.
                var errorMessage =
                    string.Format(
                        "An error occured while trying to Render the custom error view which you provided, for this HttpStatusCode. ViewPath: {0}; Message: {1}",
                        string.IsNullOrEmpty(viewPath) ? "--no view path was provided!! --" : viewPath,
                        exception.Message);

                RenderFallBackErrorViewBecauseNoneWasProvided(httpApplication, new InvalidOperationException(errorMessage, currentError));
            }
        }

        private static void RenderFallBackErrorViewBecauseNoneWasProvided(HttpApplication httpApplication,
                                                                          Exception currentError)
        {
            // TODO: keep looping through each inner exception, while/if we have one.

            const string simpleErrorMessage =
                "<html><head><title>An error has occured</title></head><body><h2>Sorry, an error occurred while processing your request.</h2><br/><br/><p><ul><li>Exception: {0}</li><li>Source: {1}</li></ul></p></body></html>";
            httpApplication.Response.Output.Write(simpleErrorMessage, currentError.Message, currentError.StackTrace);
        }
    }
}