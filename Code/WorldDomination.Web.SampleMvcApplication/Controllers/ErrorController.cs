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

        [Authorize]
        public ActionResult MustBeAuthorized()
        {
            return Content("If you can see this, then you're authorized.");
        }

        public ActionResult WhatchaTalkinBoutWillis()
        {
            // A 403 shouldn't be handled explicitly in the web.config.
            // Therefore, the defaultRedirect should be used.
            return new HttpStatusCodeResult(405, "What-cha talkin' bout Willis??");
        }

        public ActionResult AjaxThrowsAnError()
        {
            throw new InvalidOperationException("Oh noes - code went boomski. :sad panda:");
        }
    }
}