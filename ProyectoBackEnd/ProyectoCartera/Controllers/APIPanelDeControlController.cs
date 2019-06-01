using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using ProyectoCartera.App_Start;
using ProyectoCartera.Models.ModeloClases.Seguridad;
using ProyectoCartera.Models.ModeloClases.Contabilidad;
using ProyectoCartera.Models.ControladorDeDatos;
using ProyectoCartera.Models.ModeloClases;
using ProyectoCartera.Models.AccesoADatos;
using ProyectoCartera.Models.AccesoADatos;

namespace ProyectoCartera.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/APIPanelDeControl")]
    public class APIPanelDeControlController : ApiController
    {
        private DataSeguridad objDataSeguridad = null;
        private DataContabilidad objDataContabilidad = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public APIPanelDeControlController()
        {
            this.objDataSeguridad = new DataSeguridad();
            this.objDataContabilidad = new DataContabilidad();
        }

        #region "INCIDENCIAS"

        /// <summary>
        /// Realiza prueba de conexión con el api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("guardarIncidencia")]
        public Datos.resultadoObjetos guardarIncidencia(string xAsunto, string xMensaje)
        {
            Datos.resultadoObjetos _resultado = new Datos.resultadoObjetos();
            try
            {
                var identity = Thread.CurrentPrincipal.Identity;
                if (identity.IsAuthenticated)
                {
                    Solicitudes_Soporte objIncidencia = new Solicitudes_Soporte();
                    objIncidencia.Asunto = xAsunto;
                    objIncidencia.Mensaje = xMensaje;
                    objIncidencia.Usuarios = identity.Name;
                    _resultado = this.objDataContabilidad.IncidenciasGuardar(objIncidencia);
                }
            }
            catch (Exception ex)
            {
                _resultado.cargarError(ex);
            }
            return _resultado;
        }

        #endregion
        #region "CARGAR DINERO"

        /// <summary>
        /// Realiza prueba de conexión con el api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("cargarDinero")]
        public Datos.resultadoObjetos cargarDinero(string xmoneda_movimiento, string xtipo_movimiento, decimal xmonto, string xDato1, string xDato2, string xDato3, string xDato4)
        {
            Datos.resultadoObjetos _resultado = new Datos.resultadoObjetos();
            try
            {
                var identity = Thread.CurrentPrincipal.Identity;
                if (identity.IsAuthenticated)
                {
                    Movimientos objMovimiento = new Movimientos();
                    objMovimiento.moneda_movimiento = xmoneda_movimiento;
                    objMovimiento.tipo_movimiento= xtipo_movimiento;
                    objMovimiento.monto = xmonto;
                    objMovimiento.Dato1 = Datos.NullAVacio(xDato1);
                    objMovimiento.Dato2 = Datos.NullAVacio(xDato2);
                    objMovimiento.Dato3  = Datos.NullAVacio(xDato3);
                    objMovimiento.Dato4 = Datos.NullAVacio(xDato4);
                    objMovimiento.Usuario = identity.Name;
                    _resultado = this.objDataContabilidad.MovimientoGuardar(objMovimiento);
                }
            }
            catch (Exception ex)
            {
                _resultado.cargarError(ex);
            }
            return _resultado;
        }



        /// <summary>
        /// Realiza prueba de conexión con el api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsultarCuentas")]
        public Datos.resultadoObjetos ConsultarCuentas(string xmoneda_movimiento)
        {
            Datos.resultadoObjetos _resultado = new Datos.resultadoObjetos();
            try
            {
                var identity = Thread.CurrentPrincipal.Identity;
                if (identity.IsAuthenticated)
                {
                    Movimientos objMovimiento = new Movimientos();
                    objMovimiento.moneda_movimiento = xmoneda_movimiento;
                    objMovimiento.Usuario = identity.Name;
                    _resultado = this.objDataContabilidad.SaldosConsultar(objMovimiento);
                }
            }
            catch (Exception ex)
            {
                _resultado.cargarError(ex);
            }
            return _resultado;
        }


        /// <summary>
        /// Realiza prueba de conexión con el api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsultarMovimientos")]
        public Datos.resultadoObjetos ConsultarMovimientos(string xFechaInicial, string xFechaFinal, string xmoneda_movimiento)
        {
            Datos.resultadoObjetos _resultado = new Datos.resultadoObjetos();
            try
            {
                var identity = Thread.CurrentPrincipal.Identity;
                if (identity.IsAuthenticated)
                {
                    Movimientos objMovimiento = new Movimientos();
                    objMovimiento.moneda_movimiento = xmoneda_movimiento;
                    objMovimiento.Usuario = identity.Name;
                    objMovimiento.fecha_inicial = xFechaInicial;
                    objMovimiento.fecha_final = xFechaFinal;
                    _resultado = this.objDataContabilidad.MovimientosConsultar(objMovimiento);
                }
            }
            catch (Exception ex)
            {
                _resultado.cargarError(ex);
            }
            return _resultado;
        }

        #endregion

    }

}
