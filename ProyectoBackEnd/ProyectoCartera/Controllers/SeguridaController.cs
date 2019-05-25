using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoCartera.Controllers
{
    public class SeguridaController : Controller
    {
        // GET: Segurida
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Registro de usuarios
        /// </summary>
        /// <returns></returns>
        public ViewResult Registrarse() {
            return View();
        }

        /// <summary>
        /// Iniciar sesión en el sistema
        /// </summary>
        /// <returns></returns>
        public ViewResult IniciarSesion() {
            return View();
        }

    }
}