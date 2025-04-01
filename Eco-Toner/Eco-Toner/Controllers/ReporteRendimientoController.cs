using Eco_Toner.Models.DB;
using Eco_Toner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Eco_Toner.Controllers
{
    public class ReporteRendimientoController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;

        public ReporteRendimientoController(DbAb6668EcotonerContext context)
        {
            _context = context;
        }

        public IActionResult ReporteRendimiento(FiltroRendimiento filtro, bool aplicarFiltro = false)
        {
            // Obtener el usuario enviado desde el formulario (NO el de la sesión)
            ViewBag.usuario = filtro.Usuario;

            // Si no se aplica filtro, solo mostrar la vista vacía
            if (!aplicarFiltro)
            {
                return View();
            }
            
            // Validar fechas
            if (filtro.FechaInicio == null || filtro.FechaFin == null)
            {
                return BadRequest("Las fechas no pueden estar vacías.");
            }

            if (filtro.FechaFin < filtro.FechaInicio)
            {
                ModelState.AddModelError("", "La fecha fin no puede ser menor a la fecha inicio");
                filtro.FechaFin = filtro.FechaInicio;
            }

            try
            {
                // Ejecutar el procedimiento almacenado con el usuario proporcionado en el filtro
                var reporteFiltrado = _context.Database
                    .SqlQueryRaw<ReporteRendimiento>(
                        "EXEC sp_DesempenoUsuario @Usuario, @FechaInicio, @FechaFin",
                        new SqlParameter("@Usuario", filtro.Usuario) ,
                        new SqlParameter("@FechaInicio", filtro.FechaInicio) ,
                        new SqlParameter("@FechaFin", filtro.FechaFin) 
                    ).ToList();


                // Crear el modelo unificado
                var model = new ReporteDesempenoUsuarioViewModel
                {
                    Reportes = reporteFiltrado ?? new List<ReporteRendimiento>()
                };

                ViewBag.Filtro = filtro;
                ViewBag.AplicarFiltro = true;

                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en la consulta: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Filtrar(FiltroRendimiento filtro)
        {
            return RedirectToAction("ReporteRendimiento", new { filtro.Usuario, filtro.FechaInicio, filtro.FechaFin, aplicarFiltro = true });
        }

    }

}

