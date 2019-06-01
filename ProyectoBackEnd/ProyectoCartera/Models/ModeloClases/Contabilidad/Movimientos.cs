using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCartera.Models.ModeloClases.Contabilidad
{
    /// <summary>
    /// Estructura utilizada para los movimientos de la cuenta
    /// </summary>
    public class Movimientos
    {
        public long IdMovimientos { get; set; }
        public decimal monto { get; set; }
        public string moneda_movimiento { get; set; }
        public string tipo_movimiento { get; set; }
        public string NombreTipoMovimiento { get; set; }
        public DateTime fecha_hora { get; set; }
        public int Usuarios_identificacion { get; set; }
        public string Usuario { get; set; }
        public int Comisiones_Tipo_Comision { get; set; }
        public decimal Valor_Comision { get; set; }
        public decimal SaldoDespMovimiento { get; set; }
        public string Dato1 { get; set; }
        public string Dato2 { get; set; }
        public string Dato3 { get; set; }
        public string Dato4 { get; set; }

        public string fecha_inicial { get; set; }
        public string fecha_final { get; set; }
    }
}