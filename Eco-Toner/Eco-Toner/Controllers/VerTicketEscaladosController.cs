using Eco_Toner.Models;
using Eco_Toner.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Controllers
{
    public class VerTicketEscaladosController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;

        public VerTicketEscaladosController(DbAb6668EcotonerContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult VerTicketEscalados()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            // Verificamos si el usuario es nulo o vacío
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login"); // Redirigir a la página de inicio de sesión si no hay usuario
            }

            // Ejecuta el procedimiento almacenado y guarda el resultado
            var tickets = _context.Database
                .SqlQueryRaw<VerTicketEscalados>("EXEC SP_VERTICKETS_ESCALADOS @USUARIO",
                    new SqlParameter("@USUARIO", usuario))
                .ToList();

            // Pasa la lista de tickets a la vista
            return View(tickets);
        }
    }
}
