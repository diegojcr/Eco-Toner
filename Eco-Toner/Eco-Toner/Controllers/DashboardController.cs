﻿using Eco_Toner.Permisos;
using Microsoft.AspNetCore.Mvc;

namespace Eco_Toner.Controllers
{
    [ValidarSesion]
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
