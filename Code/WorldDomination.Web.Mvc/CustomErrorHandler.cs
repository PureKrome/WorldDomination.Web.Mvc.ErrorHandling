using System;
using System.Web;
using System.Web.Compilation;
using System.Web.Util;
using System.Web.WebPages;

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

            // When ever an error occurs on the Http Application, we'll now handle it, here.
            context.Error += (sender, e) =>
            {
                HttpContext.Current.ClearError();

                IWebObjectFactory webObjectFactory = BuildManager.GetObjectFactory(errorPage, true);

                WebPageBase webPageBase = webObjectFactory.CreateInstance() as WebPageBase;
                
                if (webPageBase == null)
                {
                    throw new InvalidOperationException("Failed to create an instance of the following page: " + errorPage);
                }

                var wrapper = new HttpContextWrapper(HttpContext.Current);
                var webPageContext = new WebPageContext(wrapper, null, null);

                webPageBase.ExecutePageHierarchy(webPageContext, HttpContext.Current.Response.Output);
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