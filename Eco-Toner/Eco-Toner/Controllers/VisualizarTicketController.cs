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

            // Llamar al SP y obtener los tickets del usuario
            var tecnicos = _context.Database
            .SqlQueryRaw<VerTecnico>("EXEC SP_VERTECNICO")
            .ToList();


            // Crear el modelo unificado y asegurarse de que las listas no sean null
            var model = new VisualizarTicketsViewModel
            {
                Tickets = tickets ?? new List<VisualizarTicket>(),
                Tecnicos = tecnicos ?? new List<VerTecnico>(), // Inicializar vacía si no se usa en esta vista

            };

            return View(model);

        }

        public IActionResult EscalarTicket(EscalarTicket EscalarTicket)
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            try
            {
                // Ejecutar el SP
                _context.Database.ExecuteSqlRaw(
                    "EXEC SP_ESCALARTICKET @usario, @descripcion, @usuario_asignado, @numero_orden",
                    new SqlParameter("@usario", usuario),
                    new SqlParameter("@descripcion", EscalarTicket.descripcion),
                    new SqlParameter("@usuario_asignado", EscalarTicket.usuario_asignado),
                    new SqlParameter("@numero_orden", EscalarTicket.numero_orden));
                TempData["MensajeExitoCrearTicket"] = "Ticket escalado con exito";
            }
            catch (Exception ex)
            {
                // Manejo de errores: muestra el mensaje de error en la vista
                TempData["MensajeErrorEscalarTicket"] = "No se pudo escalar el ticket: " + ex.Message;
            }
            return RedirectToAction("VisualizarTicket");
        }

        [HttpPost]
        public IActionResult AceptarTicket([FromBody] string numeroOrden)
        {
            // Obtener el usuario de la sesión
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            if (string.IsNullOrEmpty(usuario))
            {
                return BadRequest("El usuario no está autenticado.");
            }

            try
            {
                Console.WriteLine("Número de orden recibido: " + numeroOrden);

                _context.Database.ExecuteSqlRaw(
                    "EXEC SP_ACEPTAR_TICKET @USARIO, @NUMERO_ORDEN",
                    new SqlParameter("@USARIO", usuario),
                    new SqlParameter("@NUMERO_ORDEN", numeroOrden));

                return Ok("Ticket aceptado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al aceptar el ticket: " + ex.Message);
            }
        }

    }
}