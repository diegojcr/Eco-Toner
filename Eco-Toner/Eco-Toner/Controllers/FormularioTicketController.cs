using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class FormularioTicketController : Controller
    {
        public IActionResult FormularioTicket()
        {
            return View();
        }
    }
}
