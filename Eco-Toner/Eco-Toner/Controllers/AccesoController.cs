using Microsoft.AspNetCore.Mvc;
using Eco_Toner.Models;

namespace Eco_Toner.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Usuario oUsuario)
        {
            return RedirectToAction("Index","Home");
        }
    }
}
