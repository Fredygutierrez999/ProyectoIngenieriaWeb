using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCartera.Models.ModeloClases.Contabilidad
{
    /// <summary>
    /// Clase utilizada para administrar solicitudes de soporte
    /// </summary>
    public class Solicitudes_Soporte
    {
        public int IdSolicitud { get; set; }
        public string Asunto { get; set; }
        public DateTime Fecha_hora { get; set; }
        public string Mensaje { get; set; }
        public int Usuarios_identificacion { get; set; }
        public string Usuarios { get; set; }
        public string Respuesta { get; set; }
    }
}