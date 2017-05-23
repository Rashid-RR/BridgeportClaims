using System.Web.Mvc;

namespace BridgeportClaims.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Home Page";
                return View();
            }
            catch
            {
                return View();
            }
        }   
    }
}
