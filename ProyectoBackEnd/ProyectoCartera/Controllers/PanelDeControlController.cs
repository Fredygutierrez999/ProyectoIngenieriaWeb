using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoCartera.Controllers
{
    public class PanelDeControlController : Controller
    {
        // GET: PanelDeControl
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Vista utilizada por el usuario final, para reportar incidencias.
        /// </summary>
        /// <returns></returns>
        public ViewResult CreacionIncidencia()
        {
            return View();
        }

        /// <summary>
        /// Vista utilizada para enviar dinero
        /// </summary>
        /// <returns></returns>
        public ViewResult EnviarDinero()
        {
            return View();
        }


        /// <summary>
        /// Vista utilizada para consultar cuentas
        /// </summary>
        /// <returns></returns>
        public ViewResult ConsultarCuentas()
        {
            return View();
        }

        /// <summary>
        /// Vista utilizada para ver movimientos
        /// </summary>
        /// <returns></returns>
        public ViewResult VerMovimientos()
        {
            return View();
        }


        /// <summary>
        /// Vista utilizada para cargar movimientos
        /// </summary>
        /// <returns></returns>
        public ViewResult CargarDinero()
        {
            return View();
        }


        /// <summary>
        /// Vista utilizada para cargar movimientos
        /// </summary>
        /// <returns></returns>
        public ViewResult RetirarDinero()
        {
            return View();
        }

    }
}