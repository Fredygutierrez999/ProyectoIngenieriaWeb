using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCartera.Models.ModeloClases.Seguridad
{
    /// <summary>
    /// Clase utilizada para administrar datos del usuario.
    /// BD: Usuarios
    /// Creada por: FAGV
    /// Fecha creación: 22/05/2019
    /// </summary>
    public class Usuarios
    {
        public int identificacion { get; set; }
        public string tipo_identificacion { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Contrasena_Transaccion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string genero { get; set; }
        public string email { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public int Tipo_Usuario { get; set; }
        public bool AceptaTerminos { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Usuarios() {

        }

    }
}