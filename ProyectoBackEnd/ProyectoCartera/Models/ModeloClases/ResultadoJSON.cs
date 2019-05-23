using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCartera.Models.ModeloClases
{
    /// <summary>
    /// Estructura para retornar por cada proceso a realizar en el backend
    /// </summary>
    public class ResultadoJSON
    {
        public bool ResultadoProceso { get; set; }
        public string CadenaError { get; set; }
        public object objetoData { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ResultadoJSON() {
        }
    }
}