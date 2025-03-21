using Microsoft.AspNetCore.Mvc;
using Eco_Toner.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eco_Toner.Controllers
{
    public class AccesoController : Controller
    {
        private readonly Models.DB.DbAb6668EcotonerContext _context;

        public AccesoController(Models.DB.DbAb6668EcotonerContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Usuario oUsuario)
        {
            string usuario = oUsuario.User;
            string clave = oUsuario.Contra;

            var resultadoRol = _context.Database
            .SqlQueryRaw<int>(
                    "EXEC sp_ValidarUsuario @usuario, @contra",
                    new SqlParameter("@usuario", usuario),
                    new SqlParameter("@contra", clave)
                )
                .AsEnumerable()
                .FirstOrDefault();
            var resultadoString = resultadoRol.ToString();

            if (resultadoRol == 1)
            {
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else if (resultadoRol == 2)
            {
                //Si inicia sesion un operador de servicio al cliente ira a su respectivo dashboard
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("DashboardSC", "Dashboard");
            }
            else if (resultadoRol == 3)
            {
                //Si inicia sesion un tecnico remoto ira a su respectivo dashboard
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("DashboardTR", "Dashboard");
            }
            else if (resultadoRol == 4)
            {
                //Si inicia sesion un tecnico de area ira a su respectivo dashboard
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("DashboardTA", "Dashboard");
            }
            else if (resultadoRol == 5)
            {
                //Si inicia sesion un tecnico de area ira a su respectivo dashboard
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("DashboardTT", "Dashboard");
            }
            else
            {
                // Usuario no válido: mostrar mensaje de error
                ViewData["Mensaje"] = "Usuario no encontrado.";
                return View();
            }

        }
    }
}
