using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class FormularioTicketController : Controller
    {
        public IActionResult FormularioTicket()
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            return View();
        }
    }
}
