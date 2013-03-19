using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using WorldDomination.Web.Mvc;
using $rootnamespace$.App_Start;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CustomErrorHander), "PreStart")]
namespace $rootnamespace$.App_Start
{
    public static class CustomErrorHander
    {
        public static void PreStart()
        {
            // Register the custom error handling module.
            DynamicModuleUtility.RegisterModule(typeof (CustomErrorHandlingModule));
        }
    }
}