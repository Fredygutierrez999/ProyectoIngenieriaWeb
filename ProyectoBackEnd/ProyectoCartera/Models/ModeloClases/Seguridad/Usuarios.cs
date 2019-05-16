using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCartera.Models.ModeloClases.Seguridad
{

    public class Usuarios
    {
        public int ID { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
    }
}