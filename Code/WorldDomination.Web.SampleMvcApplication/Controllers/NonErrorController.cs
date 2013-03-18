using System.Web.Mvc;

namespace WorldDomination.Web.SampleMvcApplication.Controllers
{
    public class NonErrorController : Controller
    {
        public ViewResult Ok200()
        {
            return View();
        }

        public RedirectResult PermanentRedirection301()
        {
			return RedirectPermanent("https://github.com/PureKrome/WorldDomination.Web.Mvc.ErrorHandling");
        }

        public RedirectResult TemporaryRedirection302()
        {
			return Redirect("https://github.com/PureKrome/WorldDomination.Web.Mvc.ErrorHandling");
        }
    }
}