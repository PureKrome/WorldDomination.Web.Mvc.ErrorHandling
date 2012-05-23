using System.Web.Mvc;
using WorldDomination.Web.SampleMvcApplication.Models;

namespace WorldDomination.Web.SampleMvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new IndexViewModel
                            {
                                IsCustomErrorsEnabled = ControllerContext.HttpContext.IsCustomErrorEnabled,
                                IsCustomErrorsEnabledText = ControllerContext.HttpContext.IsCustomErrorEnabled
                                                                ? "The defined custom error view will be rendered."
                                                                : "To render the defined custom error, the httpErrors element needs to be set to Custom. eg. <httpErrors errorMode=\"Custom\"/>"
                            };
            return View(model);
        }
    }
}