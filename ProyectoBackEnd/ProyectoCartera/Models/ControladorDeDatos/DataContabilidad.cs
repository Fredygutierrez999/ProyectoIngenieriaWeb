using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoCartera.Models.AccesoADatos;
using ProyectoCartera.Models.ModeloClases.Seguridad;
using ProyectoCartera.Models.ModeloClases.Contabilidad;
using System.IO;
using System.Text;


namespace ProyectoCartera.Models.ControladorDeDatos
{
    /// <summary>
    /// Clase utilizda para administrar datos del modulo de contabilidad (Saldos, movimientos etc)
    /// Creado por: FAGV
    /// Fecha creación: 11/28/2018
    /// </summary>
    public class DataContabilidad : Datos
    {


        #region "INCIDENCIAS"

        /// <summary>
        /// Metodo utilizado para validar datos del usuario
        /// </summary>
        /// <param name="objUsuarios">Objeto de usuario</param>
        /// <returns></returns>
        private List<string> validaDatosIncidencia(Solicitudes_Soporte objIncidencia)
        {
            List<string> lstErrores = new List<string>();
            if (string.IsNullOrEmpty(objIncidencia.Asunto))
            {
                lstErrores.Add("Debe indicar un asunto");
            }
            if (string.IsNullOrEmpty(objIncidencia.Mensaje))
            {
                lstErrores.Add("Debe ingresar un mensaje");
            }
            return lstErrores;
        }

        /// <summary>
        /// Metodo utilizado para guardar datos de la incidencia
        /// </summary>
        /// <param name="objIncidencia"></param>
        /// <returns></returns>
        public resultadoObjetos IncidenciasGuardar(Solicitudes_Soporte objIncidencia)
        {
            resultadoObjetos _resultado = new resultadoObjetos();
            try
            {
                List<string> lstMensajes = validaDatosIncidencia(objIncidencia);
                if (lstMensajes.Count == 0)
                {
                    this.Comando = "pa_Solicitudes_Soporte_Guardar";
                    this.AgregarParametro("@Asunto", objIncidencia.Asunto);
                    this.AgregarParametro("@Mensaje", objIncidencia.Mensaje);
                    this.AgregarParametro("@Usuarios", objIncidencia.Usuarios);
                    var _resultadoDatosBD = this.TablaResultado();
                    this.asigarDatosDesdeDatatable(_resultadoDatosBD, _resultadoDatosBD.Data);
                    _resultado.set(_resultadoDatosBD);
                }
                else
                {
                    _resultado.CadenaError = _resultado.generarHMLTValidaciones(lstMensajes);
                    _resultado.setTipoMensaje(resultadoObjetos.EnumTipoMensaje.ADVERTENCIA);
                    _resultado.ResultadoProceso = false;
                }
            }
            catch (Exception ex)
            {
                _resultado.cargarError(ex);
            }
            return _resultado;
        }

        #endregion

        #region "INCIDENCIAS"

        /// <summary>
        /// Metodo utilizado para validar datos del usuario
        /// </summary>
        /// <param name="objUsuarios">Objeto de usuario</param>
        /// <returns></returns>
        private List<string> validaDatosMovimiento(Movimientos objMovimiento)
        {
            List<string> lstErrores = new List<string>();
            if (string.IsNullOrEmpty(objMovimiento.tipo_movimiento))
            {
                lstErrores.Add("Debe indicar un tipo de movimiento");
            }
            if (string.IsNullOrEmpty(objMovimiento.moneda_movimiento))
            {
                lstErrores.Add("Debe seleccionar un tipo de moneda");
            }
            if (objMovimiento.monto == 0)
            {
                lstErrores.Add("Debe indicar un monto valido");
            }
            return lstErrores;
        }

        /// <summary>
        /// Metodo utilizado para guardar movimiento de la cuenta
        /// </summary>
        /// <param name="objIncidencia"></param>
        /// <returns></returns>
        public resultadoObjetos MovimientoGuardar(Movimientos objMovimiento)
        {
            resultadoObjetos _resultado = new resultadoObjetos();
            try
            {
                List<string> lstMensajes = validaDatosMovimiento(objMovimiento);
                if (lstMensajes.Count == 0)
                {
                    this.Comando = "pa_Movimientos_Guardar";
                    this.AgregarParametro("@monto", objMovimiento.monto);
                    this.AgregarParametro("@moneda_movimiento", objMovimiento.moneda_movimiento);
                    this.AgregarParametro("@tipo_movimiento", objMovimiento.tipo_movimiento);
                    this.AgregarParametro("@Dato1", objMovimiento.Dato1);
                    this.AgregarParametro("@Dato2", objMovimiento.Dato2);
                    this.AgregarParametro("@Dato3", objMovimiento.Dato3);
                    this.AgregarParametro("@Dato4", objMovimiento.Dato4);
                    this.AgregarParametro("@Usuarios", objMovimiento.Usuario);
                    var _resultadoDatosBD = this.TablaResultado();
                    if (_resultadoDatosBD.ResultadoProceso)
                    {
                        this.asigarDatosDesdeDatatable(_resultadoDatosBD, _resultadoDatosBD.Data);
                    }
                    _resultado.set(_resultadoDatosBD);
                }
                else
                {
                    _resultado.CadenaError = _resultado.generarHMLTValidaciones(lstMensajes);
                    _resultado.setTipoMensaje(resultadoObjetos.EnumTipoMensaje.ADVERTENCIA);
                    _resultado.ResultadoProceso = false;
                }
            }
            catch (Exception ex)
            {
                _resultado.cargarError(ex);
            }
            return _resultado;
        }


        /// <summary>
        /// Metodo utilizado para guardar movimiento de la cuenta
        /// </summary>
        /// <param name="objIncidencia"></param>
        /// <returns></returns>
        public resultadoObjetos SaldosConsultar(Movimientos objMovimiento)
        {
            resultadoObjetos _resultado = new resultadoObjetos();
            try
            {
                Saldos objSaldos = new Saldos();
                this.Comando = "pa_Saldos_Consultar";
                this.AgregarParametro("@moneda_movimiento", objMovimiento.moneda_movimiento);
                this.AgregarParametro("@Usuarios", objMovimiento.Usuario);
                var _resultadoDatosBD = this.TablaSetResultado();
                if (_resultadoDatosBD.ResultadoProceso)
                {
                    DataTable dttSaldos = _resultadoDatosBD.DataSet.Tables[0];
                    DataTable dttMovimientos = _resultadoDatosBD.DataSet.Tables[1];
                    this.asigarDatosDesdeDatatable(objSaldos, dttSaldos);
                    this.asigarDatosDesdeDatatable(objSaldos.MovimientosCuenta, dttMovimientos);
                }
                _resultado.objetoData = objSaldos;
                _resultado.set(_resultadoDatosBD);
            }
            catch (Exception ex)
            {
                _resultado.cargarError(ex);
            }
            return _resultado;
        }


        /// <summary>
        /// Metodo utilizado para guardar movimiento de la cuenta
        /// </summary>
        /// <param name="objIncidencia"></param>
        /// <returns></returns>
        public resultadoObjetos MovimientosConsultar(Movimientos objMovimiento)
        {
            resultadoObjetos _resultado = new resultadoObjetos();
            try
            {
                List<Movimientos> lstMovimientos = new List<Movimientos>();
                this.Comando = "pa_Movimientos_Consultar";
                this.AgregarParametro("@moneda_movimiento", objMovimiento.moneda_movimiento);
                this.AgregarParametro("@Usuarios", objMovimiento.Usuario);

                DateTime xFechaInicial = DateTime.Now;
                if (DateTime.TryParse(objMovimiento.fecha_inicial, out xFechaInicial))
                {
                    this.AgregarParametro("@FechaInicial", Datos.NullAValDefecto(xFechaInicial));
                }
                else {
                    this.AgregarParametro("@FechaInicial", Datos.NullAValDefecto(DateTime.MinValue));
                }
                if (DateTime.TryParse(objMovimiento.fecha_final, out xFechaInicial))
                {
                    this.AgregarParametro("@FechaFinal", Datos.NullAValDefecto(xFechaInicial));
                }
                else
                {
                    this.AgregarParametro("@FechaFinal", Datos.NullAValDefecto(DateTime.MinValue));
                }
                var _resultadoDatosBD = this.TablaSetResultado();
                if (_resultadoDatosBD.ResultadoProceso)
                {
                    DataTable dttSaldos = _resultadoDatosBD.DataSet.Tables[0];
                    this.asigarDatosDesdeDatatable(lstMovimientos, dttSaldos);
                }
                _resultado.objetoData = lstMovimientos;
                _resultado.set(_resultadoDatosBD);
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