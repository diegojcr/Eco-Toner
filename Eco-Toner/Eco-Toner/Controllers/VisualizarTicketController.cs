using Eco_Toner.Models;
using Eco_Toner.Models.DB;
using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class VisualizarTicketController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;

        public VisualizarTicketController(DbAb6668EcotonerContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult VisualizarTicket()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            // Verificamos si el usuario es nulo o vacío
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login"); // Redirigir a la página de inicio de sesión si no hay usuario
            }

            // Llamar al SP y obtener los tickets del usuario
            var tickets = _context.Database
            .SqlQueryRaw<VisualizarTicket>("EXEC SP_VERTICKETS @USUARIO", new SqlParameter("@USUARIO", usuario))
            .ToList();


            // Asegurar que no sea null, en su lugar devolver una lista vacía
            if (tickets == null || tickets.Count == 0)
            {
                tickets = new List<VisualizarTicket>(); // Lista vacía para evitar errores en la vista
            }

            return View(tickets);
        }
    }
}