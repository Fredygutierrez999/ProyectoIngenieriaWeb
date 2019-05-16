using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoCartera.Controllers
{
    public class SeguridadController : Controller
    {

        /// <summary>
        /// Vista utilizada para iniciar sesion
        /// </summary>
        /// <returns></returns>
        public ViewResult IniciarSesion() {
            return View();
        }

    }
}