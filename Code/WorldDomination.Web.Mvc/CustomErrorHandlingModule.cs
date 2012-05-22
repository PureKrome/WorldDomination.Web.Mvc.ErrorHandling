using System.Web;
using System.Web.Mvc;

namespace WorldDomination.Web.Mvc
{
    public sealed class CustomErrorHandlingModule : IHttpModule
    {
        #region Implementation of IHttpModule

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            const string errorPage = "~/Views/Shared/Error.cshtml";

            context.Error += (sender, e) =>
            {
                //HttpContext.Current.ClearError();

                HttpContext.Current.Response.TrySkipIisCustomErrors = true;

                var errorController = new ErrorController();
                var controllerContext =
                    new ControllerContext(context.Context.Request.RequestContext, errorController);
                var view = new RazorView(controllerContext, errorPage, null, false, null);
                var viewModel = new ErrorViewModel();
                var tempData = new TempDataDictionary();
                var viewContext = new ViewContext(controllerContext, view,
                                                new ViewDataDictionary(viewModel), tempData,
                                                context.Response.Output);
                view.Render(viewContext, context.Response.Output);
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
    }
}