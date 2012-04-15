using System.Web.Mvc;

namespace WorldDomination.Web.SampleMvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}