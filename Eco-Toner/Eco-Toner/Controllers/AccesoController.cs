using Microsoft.AspNetCore.Mvc;
using Eco_Toner.Models;
using System.Text;
using System.Security.Cryptography;

namespace Eco_Toner.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Usuario oUsuario)
        {
            string usuario = oUsuario.Correo;
            string clave = oUsuario.Contra;
            using (var db = new Models.DB.ProyectoEcoTonerContext())
            {
                
            }

            return RedirectToAction("Index","Home");
        }

        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
