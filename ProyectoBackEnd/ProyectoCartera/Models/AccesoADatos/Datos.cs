using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Mvc;

namespace ProyectoCartera.Models.AccesoADatos
{
    /// <summary>
    /// Descripción: Clase utilizada para administrar conexiones a base de datos
    /// Fecha creación: 11/21/2018
    /// Ultima modificación: 11/21/2018
    /// Creado por: FAGV
    /// </summary>
    public class Datos : Dato_Reflectar
    {

        #region "STATIC"
        public static int intValorDefecto = -1;
        public static string strValorDefecto = "-1";
        public static string strTableDirect = "SELECT * FROM ";

        /// <summary>
        /// Valida la cadena de entrada y retorna un valor nulo
        /// </summary>
        /// <param name="xValor"></param>
        /// <returns></returns>
        public static string NullAVacio(string xValor)
        {
            return xValor == null ? string.Empty : xValor;
        }


        /// <summary>
        /// Valida la cadena de entrada y retorna un valor nulo
        /// </summary>
        /// <param name="xValor"></param>
        /// <returns></returns>
        public static int NullAVacio(int? xValor)
        {
            return xValor == null ? 0 : Convert.ToInt32(xValor);
        }


        /// <summary>
        /// Valida la cadena de entrada y retorna un valor nulo
        /// </summary>
        /// <param name="xValor"></param>
        /// <returns></returns>
        public static decimal NullAVacio(decimal? xValor)
        {
            return xValor == null ? 0 : Convert.ToDecimal(xValor);
        }


        /// <summary>
        /// Valida la cadena de entrada y retorna un valor nulo
        /// </summary>
        /// <param name="xValor"></param>
        /// <returns></returns>
        public static string NullAValDefecto(string xValor)
        {
            return xValor == null ? strValorDefecto : xValor;
        }



        /// <summary>
        /// Valida la cadena de entrada y retorna un valor nulo
        /// </summary>
        /// <param name="xValor"></param>
        /// <returns></returns>
        public static object NullAValDefecto(DateTime xValor)
        {
            if (xValor == DateTime.MinValue)
                return DBNull.Value;
            else
                return xValor;
        }

        /// <summary>
        /// Metodo utilizado para retornar valor seleccionado
        /// </summary>
        /// <param name="lstItem"></param>
        /// <param name="valorItem"></param>
        /// <returns></returns>
        public static string retornaValorLista(List<SelectListItem> lstItem, string valorItem)
        {
            foreach (SelectListItem objItem in lstItem)
            {
                if (objItem.Value == valorItem)
                {
                    return objItem.Text;
                }
            }
            return "Sin estado";
        }

        /// <summary>
        /// Valida el numero de entrada y retorna un valor nulo
        /// </summary>
        /// <param name="xValor"></param>
        /// <returns></returns>
        public static int NullAValDefecto(int xValor)
        {
            return xValor == 0 || xValor == -1 ? intValorDefecto : xValor;
        }


        /// <summary>
        /// Valida el numero de entrada y retorna un valor nulo
        /// </summary>
        /// <param name="xValor"></param>
        /// <returns></returns>
        public static long NullAValDefecto(long xValor)
        {
            return xValor == 0 || xValor == -1 ? intValorDefecto : xValor;
        }

        #endregion




        /// <summary>
        /// Clase utilizada para retornar en conjunto el resultado de ejecución de un proceso en la base de datos
        /// /// Fecha creación: 11/21/2018
        /// Ultima modificación: 11/21/2018
        /// Creado por: FAGV
        /// </summary>
        public class resultado
        {
            /// <summary>
            /// Error tecnico generado
            /// </summary>
            public string CadenaError { get; set; }

            /// <summary>
            /// Resultado del proceso
            /// </summary>
            public bool ResultadoProceso { get; set; }

            /// <summary>
            /// Resultado en datatable
            /// </summary>
            public DataTable Data { get; set; }

            /// <summary>
            /// Resultado en dataset
            /// </summary>
            public DataSet DataSet { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public resultado()
            {
                this.ResultadoProceso = true;
                this.Data = new DataTable();
                this.DataSet = new DataSet();
            }

            /// <summary>
            /// Cargar error de conexión
            /// </summary>
            /// <param name="ex"></param>
            public void cargarError(Exception ex)
            {
                this.ResultadoProceso = false;
                this.CadenaError = ex.Message;
            }
        }

        /// <summary>
        /// Clase utilizada para retornar objeto con objeto de datos
        /// </summary>
        public class resultadoObjetos
        {

            /// <summary>
            /// istado de valores para los tipos de mensaje
            /// </summary>
            public enum EnumTipoMensaje : int
            {
                ADVERTENCIA = 1,
                ERROR = 2,
                EXITO = 4
            }

            /// <summary>
            /// Tipo de mensaje interno
            /// </summary>
            private EnumTipoMensaje _TipoMensaje;

            /// <summary>
            /// Error tecnico generado
            /// </summary>
            public string CadenaError { get; set; }

            /// <summary>
            /// Resultado del proceso
            /// </summary>
            public bool ResultadoProceso { get; set; }

            /// <summary>
            /// Resultado en datatable
            /// </summary>
            public object objetoData { get; set; }

            /// <summary>
            /// Retorna el tipo de mensaje
            /// </summary>
            public int TipoMensaje
            {
                get
                {
                    int _TipoMensaje = 0;
                    switch (this._TipoMensaje)
                    {
                        case EnumTipoMensaje.EXITO:
                            _TipoMensaje = 4;
                            break;
                        case EnumTipoMensaje.ERROR:
                            _TipoMensaje = 2;
                            break;
                        case EnumTipoMensaje.ADVERTENCIA:
                            _TipoMensaje = 1;
                            break;
                    }
                    return _TipoMensaje;
                }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public resultadoObjetos()
            {
                this.ResultadoProceso = true;
                this._TipoMensaje = EnumTipoMensaje.EXITO;
            }

            /// <summary>
            /// Cambia el tipo de mensaje
            /// </summary>
            public void setTipoMensaje(EnumTipoMensaje EnTipoMensaje)
            {
                this._TipoMensaje = EnTipoMensaje;
            }

            /// <summary>
            /// Cargar error de conexión
            /// </summary>
            /// <param name="ex"></param>
            public void cargarError(Exception ex)
            {
                this.ResultadoProceso = false;
                this.CadenaError = ex.Message;
            }

            /// <summary>
            /// Asigna datos de clase resultao 
            /// </summary>
            /// <param name="objResultado"></param>
            public void set(resultado objResultado)
            {
                this.CadenaError = objResultado.CadenaError;
                this.ResultadoProceso = objResultado.ResultadoProceso;
                if (objResultado.ResultadoProceso == false)
                {
                    this.setTipoMensaje(EnumTipoMensaje.ERROR);
                }
            }


            /// <summary>
            /// Metodo utilizado para convertir listado a selectList
            /// </summary>
            /// <param name="xID">Valot del ítem</param>
            /// <param name="xNombre">Nombre del Ítem</param>
            /// <param name="xObjetoSeleccion">Valor por defecto</param>
            /// <returns></returns>
            public SelectList AListadoMVC(string xID, string xNombre, object xObjetoSeleccion)
            {
                return new SelectList((System.Collections.IEnumerable)this.objetoData, xID, xNombre, xObjetoSeleccion);
            }

            /// <summary>
            /// Metodo utilizado para generar listado LU y LI
            /// </summary>
            /// <param name="lstErrores"></param>
            /// <returns></returns>
            public string generarHMLTValidaciones(List<string> lstErrores)
            {
                string strCadena = string.Empty;
                strCadena = "<lu>";
                for (int i = 0; i < lstErrores.Count; i++)
                {
                    strCadena += "<li>" + lstErrores[i] + "</li>";
                }
                strCadena += "</lu>";
                return strCadena;
            }


        }


        #region "Variables"
        /// <summary>
        /// Variable utulizada para almacenar la conexión de la base de datos
        /// </summary>
        private string _strConexion;

        private string _strComando;

        /// <summary>
        /// Propiedad utilziada para almacenar el comando SQL
        /// </summary>
        public string Comando
        {
            get
            {
                return _strComando;
            }
            set
            {
                this.tipoProceso = CommandType.StoredProcedure;
                this._parametrosSQL.Clear();
                this._strComando = value;
            }
        }

        /// <summary>
        /// Indica el tipo de proceso que se realiza en la base de datos.
        /// Por defecto es stored Procedure
        /// </summary>
        public CommandType tipoProceso { get; set; }

        /// <summary>
        /// Parametros de base de datos
        /// </summary>
        private List<SqlParameter> _parametrosSQL { get; set; }

        /// <summary>
        /// bandera encargada de mostrar si posee datos
        /// </summary>
        public Boolean ConDatos { get; set; }
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xStrConexion"></param>
        public Datos()
        {
            this._strConexion = ConfigurationManager.ConnectionStrings["ConexionBaseDatos"].ToString();
            this.tipoProceso = CommandType.StoredProcedure;
            this._parametrosSQL = new List<SqlParameter>();
        }


        #region "Cargar metricas de SQL"

        /// <summary>
        /// Metodo utilizado para agregar parametros a Commander
        /// </summary>
        /// <param name="xNombrePatametro"></param>
        /// <param name="xValor"></param>
        public bool AgregarParametro(string xNombrePatametro, object xValor)
        {
            try
            {
                this._parametrosSQL.Add(new SqlParameter() { ParameterName = xNombrePatametro, Value = xValor });
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Envia un objeto y este se reflecta para cargar los parametros del procedimiento almacenado
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool AgregarParametro(Object objDato)
        {
            return false;
        }

        #endregion


        #region "Ejecutar SQL"


        /// <summary>
        /// Metodo utilizado para ejecutar resultado de tatatable
        /// </summary>
        /// <returns></returns>
        public resultado TablaResultado()
        {
            resultado objResultado = Resultado(true);
            return objResultado;
        }


        /// <summary>
        /// Metodo utilizado para ejecutar resultado de tataset
        /// </summary>
        /// <returns></returns>
        public resultado TablaSetResultado()
        {
            resultado objResultado = Resultado(false);
            return objResultado;
        }


        /// <summary>
        /// Metodo utilizado para ejecutar procedo en la base de datos
        /// </summary>
        /// <returns></returns>
        private resultado Resultado(bool xResultadoTabla)
        {
            resultado objResultado = new resultado();
            try
            {
                using (SqlCommand _objCommand = new SqlCommand())
                {
                    _objCommand.CommandText = this.Comando;
                    if (this.tipoProceso == CommandType.TableDirect)
                    {
                        _objCommand.CommandType = CommandType.Text;
                        _objCommand.CommandText = strTableDirect + this.Comando;
                    }
                    else
                    {
                        _objCommand.CommandType = this.tipoProceso;
                    }
                    _objCommand.Parameters.AddRange(this._parametrosSQL.ToArray());
                    using (SqlConnection _objConexion = new SqlConnection(this._strConexion))
                    {
                        _objCommand.Connection = _objConexion;
                        _objConexion.Open();
                        SqlDataAdapter _objAdapter = new SqlDataAdapter(_objCommand);
                        if (xResultadoTabla)
                        {
                            _objAdapter.Fill(objResultado.Data);
                            ConDatos = objResultado.Data.Rows.Count > 0;
                        }
                        else
                        {
                            _objAdapter.Fill(objResultado.DataSet);
                            ConDatos = objResultado.DataSet.Tables[0].Rows.Count > 0;
                        }
                        _objAdapter.Dispose();
                        _objConexion.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                objResultado.cargarError(ex);
            }
            finally
            {
                this._parametrosSQL.Clear();
            }
            return objResultado;
        }

        #endregion




    }
}