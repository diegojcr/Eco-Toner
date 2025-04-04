using Eco_Toner.Models;
using Eco_Toner.Models.DB;
using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class IngresoClienteController : Controller
    {
        private readonly DbAb6668EcotonerContext _context;

        public IngresoClienteController(DbAb6668EcotonerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult IngresoCliente()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Home");
            }

            var model = new IngresoCliente
            {
                Empresas = _context.Database
                    .SqlQueryRaw<string>("EXEC SP_VER_EMPRESA")
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IngresoCliente(IngresoCliente cliente)
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            if (ModelState.IsValid)
            {
                try
                {
                    // Ejecutar SP para guardar el cliente
                    _context.Database.ExecuteSqlRaw(
                        "EXEC SP_NUEVO_CLIENTE @Nombre, @Apellido, @Correo, @Empresa, @Oficina",
                        new SqlParameter("@Nombre", cliente.Nombre),
                        new SqlParameter("@Apellido", cliente.Apellido),
                        new SqlParameter("@Correo", cliente.Correo),
                        new SqlParameter("@Empresa", cliente.EmpresaSeleccionada),
                        new SqlParameter("@Oficina", cliente.OficinaSeleccionada));

                    TempData["MensajeExitoRegistro"] = "Cliente registrado exitosamente!";
                    return RedirectToAction("IngresoCliente");
                }
                catch (Exception ex)
                {
                    TempData["MensajeErrorRegistro"] = "Error al registrar: " + ex.Message;
                }
            }

            // Recargar empresas si hay errores
            cliente.Empresas = _context.Database
                .SqlQueryRaw<string>("EXEC SP_VER_EMPRESA")
                .ToList();

            return View(cliente);
        }

        [HttpGet]
        public JsonResult ObtenerOficinas(string empresa)
        {
            try
            {
                var oficinas = _context.Database
                    .SqlQueryRaw<string>("EXEC SP_VER_OFICINAS @EMPRESA",
                        new SqlParameter("@EMPRESA", empresa))
                    .ToList();

                return Json(new { success = true, data = oficinas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}