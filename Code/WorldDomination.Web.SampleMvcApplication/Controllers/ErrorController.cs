using System;
using System.Web.Mvc;

namespace WorldDomination.Web.SampleMvcApplication.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Test()
        {
            throw new InvalidOperationException("This is a test of our error handling.");
        }
    }
}