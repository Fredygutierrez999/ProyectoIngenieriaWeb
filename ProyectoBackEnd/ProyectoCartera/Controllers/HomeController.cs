using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoCartera.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Página de bienvenida
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
             

        /// <summary>
        /// Vista de contactarnos
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }


        /// <summary>
        /// Vista de terminos de uso
        /// </summary>
        /// <returns></returns>
        public ViewResult TerminosDeUso() {
            return View();
        }


        /// <summary>
        /// Vista de terminos de uso
        /// </summary>
        /// <returns></returns>
        public ViewResult PoliticaPrivacidad()
        {
            return View();
        }

    }
}