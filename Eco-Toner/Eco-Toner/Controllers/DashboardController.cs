using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            return View();
        }
        public IActionResult DashboardSC()
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            return View();
        }
        public IActionResult DashboardTR()
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            return View();
        }
        public IActionResult DashboardTA()
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            return View();
        }
        public IActionResult DashboardTT()
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            return View();
        }
    }
}
