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

            var resultado = _context.Database
            .SqlQueryRaw<int>(
                    "EXEC sp_ValidarUsuario @usuario, @contra",
                    new SqlParameter("@usuario", usuario),
                    new SqlParameter("@contra", clave)
                )
                .AsEnumerable()
                .FirstOrDefault();
            var resultadoString = resultado.ToString();

            if (resultado > 0)
            {
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("Dashboard", "Dashboard");
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
