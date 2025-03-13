using Microsoft.AspNetCore.Mvc;

namespace Eco_Toner.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
