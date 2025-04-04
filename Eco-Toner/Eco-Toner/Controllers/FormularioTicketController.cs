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


        public async Task<IActionResult> FormularioTicket()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            // Obtener técnicos y clientes
            var tecnicos = await _context.Database
                .SqlQueryRaw<TecnicoViewModel>("EXEC SP_VERTECNICO")
                .AsNoTracking()
                .ToListAsync();

            var clientes = await _context.Database
                .SqlQueryRaw<ClienteViewModel>("EXEC SP_VER_CLEINTE")
                .AsNoTracking()
                .ToListAsync();

            var model = new FormularioTicketViewModel
            {
                CrearTicket = new CrearTicket(),
                Tecnicos = tecnicos,
                Clientes = clientes
            };

            return View(model);
        }

        [HttpPost]
        public async Task <IActionResult> FormularioTicket(FormularioTicketViewModel model)
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
                new SqlParameter("@CORREO_CLIENTE", model.CrearTicket.Correo_Cliente),
                new SqlParameter("@SERIE_IMPRESORA", model.CrearTicket.Serie_Impresora),
                new SqlParameter("@CORREO_TECNICO", model.CrearTicket.Correo_Tecnico),
                new SqlParameter("@DESCRIPCION", model.CrearTicket.Descripcion));
                // Enviar correo al técnico
                string asunto = "Nuevo Ticket Asignado";
                string mensaje = $"Hola, se te ha asignado un nuevo ticket.<br><br>" +
                                 $"<b>Cliente:</b> {model.CrearTicket.Correo_Cliente} <br>" +
                                 $"<b>Impresora:</b> {model.CrearTicket.Serie_Impresora} <br>" +
                                 $"<b>Descripción:</b> {model.CrearTicket.Descripcion} <br><br>" +
                                 $"Por favor, revisa el sistema.";

                await _emailService.EnviarCorreoAsync("jdanyalvarado43@gmail.com", asunto, mensaje);

                //Enviar correo a cliente
                string asuntoCliente = "Nuevo Ticket registrado";
                string mensajeCliente = $"Hola, se ha ingresado tu caso en el sistema<br><br>" +
                                 $"<b>Cliente:</b> {model.CrearTicket.Correo_Cliente} <br>" +
                                 $"<b>Impresora:</b> {model.CrearTicket.Serie_Impresora} <br>" +
                                 $"<b>Descripción:</b> {model.CrearTicket.Descripcion} <br><br>" +
                                 $"Gracias por confiar en nosotros";

                await _emailService.EnviarCorreoAsync("analisisejemplo0@gmail.com", asuntoCliente, mensajeCliente);

                TempData["MensajeExitoCrearTicket"] = "Ticket creado y notificación enviada al técnico y al cliente";
                return RedirectToAction("FormularioTicket");
            }
            catch (Exception ex)
            {
                TempData["MensajeErrorCrearTicket"] = "No se pudo crear el ticket: " + ex.Message;
                model.Tecnicos = await _context.Database.SqlQueryRaw<TecnicoViewModel>("EXEC SP_VERTECNICO").ToListAsync();
                model.Clientes = await _context.Database.SqlQueryRaw<ClienteViewModel>("EXEC SP_VER_CLEINTE").ToListAsync();
                return View(model);
            }
        }
    }
}
