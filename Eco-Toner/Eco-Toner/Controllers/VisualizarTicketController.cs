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
    public class VisualizarTicketController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;
        private readonly EmailService _emailService;

        public VisualizarTicketController(DbAb6668EcotonerContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

        public async Task<IActionResult> EscalarTicket(EscalarTicket EscalarTicket)
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
                // 2. Obtener información para el correo
                var infoCorreo = (await _context.Database
                    .SqlQueryRaw<InfoCorreoEscalado>(
                        "EXEC SP_INFO_CORREO_ESCALADO @USUARIO_ASIGNADO, @NUMERO_ORDEN, @USUARIO_PREVIO",
                        new SqlParameter("@USUARIO_ASIGNADO", EscalarTicket.usuario_asignado),
                        new SqlParameter("@NUMERO_ORDEN", EscalarTicket.numero_orden),
                        new SqlParameter("@USUARIO_PREVIO", usuario))
                    .ToListAsync())
                    .FirstOrDefault();

                if (infoCorreo != null)
                {
                    // 3. Enviar correo al cliente
                    string asuntoCliente = $"Ticket Escalado - {EscalarTicket.numero_orden}";
                    string mensajeCliente = $@"
                <h2>Estimado(a) {infoCorreo.NOMBRECLIENTE}</h2>
                <p>Le informamos que su ticket ha sido escalado a un especialista.</p>
                
                <h3>Detalles del Ticket</h3>
                <p><strong>Número de Ticket:</strong> {EscalarTicket.numero_orden}</p>
                <p><strong>Técnico Anterior:</strong> {infoCorreo.NOMBREPREVIO} {infoCorreo.APELLIDOPREVIO}</p>
                <p><strong>Nuevo Técnico:</strong> {infoCorreo.NOMBRETECNICO} {infoCorreo.APELLIDOTECNICO}</p>
                <p><strong>Razón del Escalamiento:</strong></p>
                <p>{EscalarTicket.descripcion}</p>
                
                <p>El nuevo técnico se pondrá en contacto con usted si requiere información adicional.</p>
                <p>Gracias por su paciencia y comprensión.</p>
                <p>Atentamente,<br>El Equipo de Soporte Técnico</p>";

                    await _emailService.EnviarCorreoAsync("analisisejemplo0@gmail.com", asuntoCliente, mensajeCliente);

                    // 4. Enviar correo al nuevo técnico
                    string asuntoTecnico = $"Ticket Escalado a tu nombre - {EscalarTicket.numero_orden}";
                    string mensajeTecnico = $@"
                <h2>Ticket Escalado</h2>
                <p>Se te ha asignado un ticket escalado por {infoCorreo.NOMBREPREVIO} {infoCorreo.APELLIDOPREVIO}.</p>
                
                <h3>Detalles del Ticket</h3>
                <p><strong>Número:</strong> {EscalarTicket.numero_orden}</p>
                <p><strong>Cliente:</strong> {infoCorreo.NOMBRECLIENTE}</p>
                <p><strong>Descripción:</strong></p>
                <p>{infoCorreo.DESCRIPCION}</p>
                <p><strong>Razón del Escalamiento:</strong></p>
                <p>{EscalarTicket.descripcion}</p>
                
                <p>Por favor, revisa el sistema para atender este ticket lo antes posible.</p>";

                    await _emailService.EnviarCorreoAsync("jdanyalvarado43@gmail.com", asuntoTecnico, mensajeTecnico);
                }

                TempData["MensajeExitoCrearTicket"] = "Ticket escalado con éxito y notificaciones enviadas";
                return RedirectToAction("VisualizarTicket");
            }
            catch (Exception ex)
            {
                // Manejo de errores: muestra el mensaje de error en la vista
                TempData["MensajeErrorEscalarTicket"] = "No se pudo escalar el ticket: " + ex.Message;
            }
            return RedirectToAction("VisualizarTicket");
        }

        [HttpPost]
        public async Task<IActionResult> AceptarTicket([FromBody] string numeroOrden)
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

                // 2. Obtener información completa para el correo
                var infoCorreo = (await _context.Database
            .SqlQueryRaw<InfoCorreoAceptacion>(
                "EXEC SP_INFO_CORREO @USUARIO_ASIGNADO, @NUMERO_ORDEN",
                new SqlParameter("@USUARIO_ASIGNADO", usuario),
                new SqlParameter("@NUMERO_ORDEN", numeroOrden))
            .ToListAsync()) // Convertir a lista asincrónicamente
            .FirstOrDefault(); // Luego tomar el primer elemento

                if (infoCorreo != null && !string.IsNullOrEmpty(infoCorreo.CORREOCLIENTE))
                {
                    // 3. Enviar correo al cliente
                    string asunto = $"Ticket Aceptado - {numeroOrden}";
                    string mensaje = $@"
                <h2>Estimado(a) {infoCorreo.NOMBRECLIENTE}</h2>
                <p>Le informamos que su ticket ha sido aceptado por nuestro equipo técnico.</p>
                
                <h3>Detalles del Ticket</h3>
                <p><strong>Número de Ticket:</strong> {numeroOrden}</p>
                <p><strong>Técnico Asignado:</strong> {infoCorreo.NOMBRETECNICO} {infoCorreo.APELLIDOTECNICO}</p>
                <p><strong>Estado Actual:</strong> {infoCorreo.ESTADO}</p>
                <p><strong>Fecha de Aceptación:</strong> {infoCorreo.FECHAACEPTADO?.ToString("dd/MM/yyyy HH:mm")}</p>
                <p><strong>Descripción del Problema:</strong></p>
                <p>{infoCorreo.DESCRIPCION}</p>
                
                <p>Nuestro técnico se pondrá en contacto con usted si requiere información adicional.</p>
                <p>Gracias por confiar en nuestros servicios.</p>
                <p>Atentamente,<br>El Equipo de Soporte Técnico</p>";

                    await _emailService.EnviarCorreoAsync("analisisejemplo0@gmail.com", asunto, mensaje);
                }
                return Ok(new { success = true, message = "Ticket aceptado y notificación enviada al cliente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al aceptar el ticket: " + ex.Message);
            }
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> FinalizarTicket([FromBody] string numeroOrden)
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            if (string.IsNullOrEmpty(usuario))
            {
                return BadRequest("El usuario no está autenticado.");
            }

            try
            {
                // 1. Finalizar el ticket
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_FINALIZAR_TICKETS @usario, @numero_orden",
                    new SqlParameter("@usario", usuario),
                    new SqlParameter("@numero_orden", numeroOrden));

                // 2. Obtener información para el correo
                var infoCorreo = (await _context.Database
                    .SqlQueryRaw<InfoCorreoFinalizado>(
                        "EXEC SP_INFO_CORREO_FINALIZADO @USUARIO_ASIGNADO, @NUMERO_ORDEN",
                        new SqlParameter("@USUARIO_ASIGNADO", usuario),
                        new SqlParameter("@NUMERO_ORDEN", numeroOrden))
                    .ToListAsync())
                    .FirstOrDefault();

                if (infoCorreo != null && !string.IsNullOrEmpty(infoCorreo.CORREOCLIENTE))
                {
                    // 3. Enviar correo al cliente
                    string asunto = $"Ticket Finalizado - {numeroOrden}";
                    string mensaje = $@"
                <h2>Estimado(a) {infoCorreo.NOMBRECLIENTE}</h2>
                <p>Nos complace informarle que su ticket ha sido finalizado.</p>
                
                <h3>Detalles del Ticket</h3>
                <p><strong>Número de Ticket:</strong> {numeroOrden}</p>
                <p><strong>Técnico Responsable:</strong> {infoCorreo.NOMBRETECNICO} {infoCorreo.APELLIDOTECNICO}</p>
                <p><strong>Estado Final:</strong> {infoCorreo.ESTADO}</p>
                <p><strong>Fecha de Finalización:</strong> {infoCorreo.FECHAFINAL?.ToString("dd/MM/yyyy HH:mm")}</p>
                <p><strong>Descripción del Servicio:</strong></p>
                <p>{infoCorreo.DESCRIPCION}</p>
                
                <p>Si tiene alguna pregunta adicional o necesita más asistencia, no dude en contactarnos.</p>
                <p>Gracias por confiar en nuestros servicios.</p>
                <p>Atentamente,<br>El Equipo de Soporte Técnico</p>";

                    await _emailService.EnviarCorreoAsync("analisisejemplo0@gmail.com", asunto, mensaje);
                }

                return Ok(new { success = true, message = "Ticket finalizado y notificación enviada al cliente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al finalizar el ticket: " + ex.Message });
            }
        }

    }
}