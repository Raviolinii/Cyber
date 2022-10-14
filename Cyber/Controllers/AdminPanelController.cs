using Microsoft.AspNetCore.Mvc;

namespace Cyber.Controllers
{
    public class AdminPanelController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
