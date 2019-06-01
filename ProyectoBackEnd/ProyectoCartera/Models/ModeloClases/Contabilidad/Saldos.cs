using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCartera.Models.ModeloClases.Contabilidad
{
    public class Saldos
    {
        public long IdSaldos { get; set; }
        public string tipo_saldo  { get; set; }
        public decimal balance { get; set; }
        public long Usuarios_identificacion { get; set; }
        public List<Movimientos> MovimientosCuenta { get; set; }
        public decimal ConversionADolar { get; set; }
        public decimal SaldoConvertido { get; set; }
        public string NumeroCuenta { get; set; }

        public Saldos() {
            this.MovimientosCuenta = new List<Movimientos>();
        }

    }
}