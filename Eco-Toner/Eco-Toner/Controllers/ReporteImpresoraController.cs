using Eco_Toner.Models;
using Eco_Toner.Models.DB;
using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Controllers
{

    [ValidarSesion]
    public class ReporteImpresoraController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;

        public ReporteImpresoraController(DbAb6668EcotonerContext context)
        {
            _context = context;
        }

        public IActionResult ImpresorasConErrores(FiltroFechas filtro, bool aplicarFiltro = false)
        {
            //Obtener el usuario de la sesion
            var usuario = HttpContext.Session.GetString("Usuario");
            //Pasar ese usuario a la vista
            ViewBag.usuario = usuario;

            



            // Cuando no se ha aplicado filtro (primera vez que entra)
            if (!aplicarFiltro)
            {
                var reporteCompleto = _context.Database
                    .SqlQueryRaw<ReporteImpresora>("EXEC SP_REPORTEIMPRESORAS")
                    .AsEnumerable()
                    .ToList();
                var model = new ReporteImpresoraViewModel
                {
                    ReporteImpresora = reporteCompleto ?? new List<ReporteImpresora>(),
                };
                ViewBag.Filtro = new FiltroFechas();
                return View(model);
            }
            else
            {
                // Validar fechas
                if (filtro.FechaFin < filtro.FechaInicio)
                {
                    ModelState.AddModelError("", "La fecha fin no puede ser menor a la fecha inicio");
                    filtro.FechaFin = filtro.FechaInicio;
                }

                // Filtrar por fechas
                var reporteFiltrado = _context.Database
                    .SqlQueryRaw<ReporteImpresora>(
                        "EXEC SP_REPORTEIMPRESORAS_FECHAS @FECHA_INICIO, @FECHA_FIN",
                        new SqlParameter("@FECHA_INICIO", filtro.FechaInicio),
                        new SqlParameter("@FECHA_FIN", filtro.FechaFin)
                    )
                    .AsEnumerable()
                    .ToList();
                var model = new ReporteImpresoraViewModel
                {
                    ReporteImpresora = reporteFiltrado ?? new List<ReporteImpresora>(),
                };

                ViewBag.Filtro = filtro;
                ViewBag.AplicarFiltro = true;
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult Filtrar(FiltroFechas filtro)
        {
            return RedirectToAction("ImpresorasConErrores", new { filtro.FechaInicio, filtro.FechaFin, aplicarFiltro = true });
        }
    }
}
