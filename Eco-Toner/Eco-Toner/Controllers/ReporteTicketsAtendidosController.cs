using Eco_Toner.Models;
using Eco_Toner.Models.DB;
using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class ReporteTicketsAtendidosController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;

        public ReporteTicketsAtendidosController(DbAb6668EcotonerContext context)
        {
            _context = context;
        }
        public IActionResult TicketsAtendidosSC(FiltroFechasTicketsAtendidos filtro, bool aplicarFiltro = false)
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;

            // Cuando no se ha aplicado filtro (primera vez que entra)
            if (!aplicarFiltro)
            {
                return View();
            }
            else
            {
                // Validar fechas
                if (filtro.FechaFin < filtro.FechaInicio)
                {
                    ModelState.AddModelError("", "La fecha fin no puede ser menor a la fecha inicio");
                    filtro.FechaFin = filtro.FechaInicio;
                }
                // Validar fechas
                if (filtro.FechaInicio == null || filtro.FechaFin == null)
                {
                    return BadRequest("Las fechas no pueden estar vacías.");
                }
                try
                {
                    List<ReporteTicketsAtendidos> reporteFiltrado;

                    // Si hay número de orden, usar ese filtro
                    if (!string.IsNullOrEmpty(filtro.NumeroOrden))
                    {
                        reporteFiltrado = _context.Database
                            .SqlQueryRaw<ReporteTicketsAtendidos>(
                                "EXEC SP_REPORTE_TICKETS_NUMERO_ORDEN @NUMERO_ORDEN",
                                new SqlParameter("@NUMERO_ORDEN", filtro.NumeroOrden)
                            )
                            .AsEnumerable()
                            .ToList();
                    }
                    else // Si no, filtrar por fechas
                    {
                        reporteFiltrado = _context.Database
                            .SqlQueryRaw<ReporteTicketsAtendidos>(
                                "EXEC SP_REPORTE_TICKETS @FECHA_INICIO, @FECHA_FIN",
                                new SqlParameter("@FECHA_INICIO", filtro.FechaInicio),
                                new SqlParameter("@FECHA_FIN", filtro.FechaFin)
                            )
                            .AsEnumerable()
                            .ToList();
                    }

                    int cantidadTickets = reporteFiltrado.Count;
                    var model = new ReporteTicketsAtendidosViewModel
                    {
                        Reportes = reporteFiltrado ?? new List<ReporteTicketsAtendidos>(),
                    };

                    ViewBag.Filtro = filtro;
                    ViewBag.AplicarFiltro = true;
                    ViewBag.CantidadTickets = cantidadTickets;

                    return View(model);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error en la consulta: " + ex.Message);
                }

            }


        }
        public IActionResult TicketsAtendidos(FiltroFechasTicketsAtendidos filtro, bool aplicarFiltro = false)
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;

            // Cuando no se ha aplicado filtro (primera vez que entra)
            if (!aplicarFiltro)
            {
                return View();
            }
            else
            {
                // Validar fechas
                if (filtro.FechaFin < filtro.FechaInicio)
                {
                    ModelState.AddModelError("", "La fecha fin no puede ser menor a la fecha inicio");
                    filtro.FechaFin = filtro.FechaInicio;
                }
                // Validar fechas
                if (filtro.FechaInicio == null || filtro.FechaFin == null)
                {
                    return BadRequest("Las fechas no pueden estar vacías.");
                }
                try {
                    // Filtrar por fechas
                    var reporteFiltrado = _context.Database
                        .SqlQueryRaw<ReporteTicketsAtendidos>(
                            "EXEC SP_REPORTE_TICKETS @FECHA_INICIO, @FECHA_FIN",
                            new SqlParameter("@FECHA_INICIO", filtro.FechaInicio),
                            new SqlParameter("@FECHA_FIN", filtro.FechaFin)
                        )
                        .AsEnumerable()
                        .ToList();
                    int cantidadTickets = reporteFiltrado.Count;
                    // Crear el modelo unificado y asegurarse de que las listas no sean null
                    var model = new ReporteTicketsAtendidosViewModel
                    {
                        Reportes = reporteFiltrado ?? new List<ReporteTicketsAtendidos>(),
                    };
                    ViewBag.Filtro = filtro;
                    ViewBag.AplicarFiltro = true;
                    //mandar a la vista la cantidad de tickets que hay
                    ViewBag.CantidadTickets = cantidadTickets;
                    return View(model);
                } catch(Exception ex)
                {
                    return StatusCode(500, "Error en la consulta: " + ex.Message);
                }

            }

            
        }

        [HttpGet]
        public IActionResult ObtenerDetalleTicket(string numeroOrden)
        {
            try
            {
                var detalle = _context.Database
                    .SqlQueryRaw<DetalleTicket>(
                        "EXEC SP_VER_DETALLE_TICKET @NUMERO_ORDEN",
                        new SqlParameter("@NUMERO_ORDEN", numeroOrden)
                    )
                    .AsEnumerable()
                    .FirstOrDefault();

                if (detalle == null)
                {
                    return NotFound("No se encontraron detalles para este ticket.");
                }

                return PartialView("_DetalleTicketPartial", detalle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener el detalle: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Filtrar(FiltroFechasTicketsAtendidos filtro)
        {
            return RedirectToAction("TicketsAtendidos", new
            {
                filtro.FechaInicio,
                filtro.FechaFin,
                filtro.NumeroOrden,
                aplicarFiltro = true
            });
        }
        public IActionResult FiltrarSC(FiltroFechasTicketsAtendidos filtro)
        {
            return RedirectToAction("TicketsAtendidosSC", new
            {
                filtro.FechaInicio,
                filtro.FechaFin,
                filtro.NumeroOrden,
                aplicarFiltro = true
            });
        }
    }
}
