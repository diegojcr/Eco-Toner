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
            // Validación de usuario
            var usuario = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.usuario = usuario;

            // Obtener todas las empresas
            var empresas = _context.Database.SqlQueryRaw<string>("EXEC SP_VER_EMPRESA").ToList();

            // Crear modelo con las empresas
            var model = new IngresoCliente
            {
                Empresas = empresas
            };

            // Obtener empresa seleccionada (prioridad: Session > TempData)
            var empresaSeleccionada = HttpContext.Session.GetString("EmpresaSeleccionada") ??
                                    TempData["EmpresaSeleccionada"]?.ToString();

            // Si hay empresa seleccionada Y existe en la lista de empresas
            if (!string.IsNullOrEmpty(empresaSeleccionada) && empresas.Contains(empresaSeleccionada))
            {
                model.EmpresaSeleccionada = empresaSeleccionada;
                model.Oficinas = ObtenerOficinas(empresaSeleccionada);

                // Persistir en Session para próximas solicitudes
                HttpContext.Session.SetString("EmpresaSeleccionada", empresaSeleccionada);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IngresoCliente(IngresoCliente cliente, string accion)
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.usuario = usuario;

            // Siempre cargar las empresas disponibles
            cliente.Empresas = _context.Database.SqlQueryRaw<string>("EXEC SP_VER_EMPRESA").ToList();

            if (accion == "BuscarOficinas")
            {
                if (string.IsNullOrEmpty(cliente.EmpresaSeleccionada))
                {
                    ModelState.AddModelError("EmpresaSeleccionada", "Debe seleccionar una empresa");
                    return View(cliente);
                }

                // Validar que la empresa seleccionada exista en la lista
                if (!cliente.Empresas.Contains(cliente.EmpresaSeleccionada))
                {
                    ModelState.AddModelError("EmpresaSeleccionada", "Empresa no válida");
                    return View(cliente);
                }

                // Persistir la selección
                HttpContext.Session.SetString("EmpresaSeleccionada", cliente.EmpresaSeleccionada);

                // Obtener oficinas
                cliente.Oficinas = ObtenerOficinas(cliente.EmpresaSeleccionada);

                return View(cliente);
            }

            if (accion == "RegistrarCliente")
            {
                if (!ModelState.IsValid)
                {
                    // Recargar oficinas si hay empresa seleccionada
                    if (!string.IsNullOrEmpty(cliente.EmpresaSeleccionada))
                    {
                        cliente.Oficinas = ObtenerOficinas(cliente.EmpresaSeleccionada);
                    }
                    return View(cliente);
                }

                try
                {
                    _context.Database.ExecuteSqlRaw(
                        "EXEC SP_NUEVO_CLIENTE @NOMBRE, @APELLIDO, @CORREO, @EMPRESA, @OFICINA",
                        new SqlParameter("@NOMBRE", cliente.Nombre),
                        new SqlParameter("@APELLIDO", cliente.Apellido),
                        new SqlParameter("@CORREO", cliente.Correo),
                        new SqlParameter("@EMPRESA", cliente.EmpresaSeleccionada),
                        new SqlParameter("@OFICINA", cliente.OficinaSeleccionada));

                    // Limpiar selección después de registrar
                    HttpContext.Session.Remove("EmpresaSeleccionada");

                    TempData["MensajeExitoRegistro"] = "Cliente registrado exitosamente!";
                    return RedirectToAction("IngresoCliente");
                }
                catch (Exception ex)
                {
                    TempData["MensajeErrorRegistro"] = $"Error al registrar: {ex.Message}";

                    // Recargar oficinas si hay empresa seleccionada
                    if (!string.IsNullOrEmpty(cliente.EmpresaSeleccionada))
                    {
                        cliente.Oficinas = ObtenerOficinas(cliente.EmpresaSeleccionada);
                    }
                    return View(cliente);
                }
            }

            // Si no es ninguna acción conocida, redirigir al GET
            return RedirectToAction("IngresoCliente");
        }

        private List<string> ObtenerOficinas(string empresa)
        {
            return _context.Database
                .SqlQueryRaw<string>("EXEC SP_VER_OFICINAS @EMPRESA",
                    new SqlParameter("@EMPRESA", empresa))
                .ToList();
        }


    }
}