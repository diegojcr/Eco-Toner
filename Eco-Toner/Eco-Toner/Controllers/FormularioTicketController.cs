using Eco_Toner.Models;
using Eco_Toner.Models.DB;
using Eco_Toner.Permisos;
using Eco_Toner.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class FormularioTicketController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;
        private readonly EmailService _emailService;

        public FormularioTicketController(DbAb6668EcotonerContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult FormularioTicket()
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> FormularioTicket(CrearTicket crearTicket)
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;
            try
            {
                // Ejecutar el SP
                _context.Database.ExecuteSqlRaw(
                    "EXEC SP_NUEVOTICKET @USUARIO, @CORREO_CLIENTE, @SERIE_IMPRESORA, @CORREO_TECNICO, @DESCRIPCION",
                    new SqlParameter("@USUARIO", usuario),
                    new SqlParameter("@CORREO_CLIENTE", crearTicket.Correo_Cliente),
                    new SqlParameter("@SERIE_IMPRESORA", crearTicket.Serie_Impresora),
                    new SqlParameter("@CORREO_TECNICO", crearTicket.Correo_Tecnico),
                    new SqlParameter("@DESCRIPCION", crearTicket.Descripcion));
                // Enviar correo al técnico
                string asunto = "Nuevo Ticket Asignado";
                string mensaje = $"Hola, se te ha asignado un nuevo ticket.<br><br>" +
                                 $"<b>Cliente:</b> {crearTicket.Correo_Cliente} <br>" +
                                 $"<b>Impresora:</b> {crearTicket.Serie_Impresora} <br>" +
                                 $"<b>Descripción:</b> {crearTicket.Descripcion} <br><br>" +
                                 $"Por favor, revisa el sistema.";

                await _emailService.EnviarCorreoAsync("jdanyalvarado43@gmail.com", asunto, mensaje);

                TempData["MensajeExitoCrearTicket"] = "Ticket creado y notificación enviada al técnico.";
                return View();
            }
            catch (Exception ex)
            {
                // Manejo de errores: muestra el mensaje de error en la vista
                TempData["MensajeErrorCrearTicket"] = "No se pudo crear el ticket: " + ex.Message;
                return View();
            }
        }
    }
}
