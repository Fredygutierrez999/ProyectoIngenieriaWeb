using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;

namespace ProyectoCartera.Models.AccesoADatos
{
    /// <summary>
    /// 
    /// </summary>
    public class Dato_Reflectar
    {

        public class validacionObjetos
        {
            public int IDAppNetEstados { get; set; }  /*  */
            public string PropiedadRequerida { get; set; }  /*  */
            public string ValorNoContenido { get; set; }  /*  */
            public string ValorContenido { get; set; }  /*  */
            public string MensajeValidacion { get; set; } /*  */
            public string PropiedadRequerida_Padre { get; set; }
        }


        /// <summary>
        /// Metodo encargado de obtener el listado de propiedades del objeto a un diccionario
        /// </summary>
        /// <param name="xObj"></param>
        /// <returns></returns>
        private static Dictionary<string, MethodInfo> GetDiccionarioMetodos(object xObj)
        {
            Dictionary<string, MethodInfo> diccionarioDatos = new Dictionary<string, MethodInfo>();
            foreach (MethodInfo item in ((Type)xObj.GetType()).GetMethods())
            {
                if (!diccionarioDatos.ContainsKey(item.Name))
                {
                    diccionarioDatos.Add(item.Name, item);
                }
            }
            return diccionarioDatos;
        }

        /// <summary>
        /// Metodo encargado de obtener el listado de propiedades del objeto a un diccionario
        /// </summary>
        /// <param name="xObj"></param>
        /// <returns></returns>
        private static Dictionary<string, PropertyInfo> GetDiccionarioPropiedades(object xObj)
        {
            Dictionary<string, PropertyInfo> diccionarioDatos = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo item in ((Type)xObj.GetType()).GetProperties())
            {
                diccionarioDatos.Add(item.Name, item);
            }
            return diccionarioDatos;
        }

        /// <summary>
        /// Metodo utilizado para mapear y copiar datos de un data table
        /// </summary>
        /// <param name="_Objetos"></param>
        /// <param name="dttDatos"></param>
        public void asigarDatosDesdeDatatable(object _Objetos, DataTable dttDatos)
        {
            try
            {
                /*OBTIENE PROPIEDADES DEL LISTADO (NOMBRE PROPIEDAD Y TIPO DE DATO) (MAPEO)*/
                /*Lista*/
                if (_Objetos.GetType().IsGenericType)
                {
                    /*OBTIENE PROPIEDADES DEL LISTADO (NOMBRE PROPIEDAD Y TIPO DE DATO) (MAPEO)*/
                    Dictionary<string, MethodInfo> diccionarioMetodos = GetDiccionarioMetodos(_Objetos);
                    var objItemTemporal = diccionarioMetodos["get_Item"];
                    Type objTipoItem = objItemTemporal.ReturnType;
                    for (var i = 0; i < dttDatos.Rows.Count; i++)
                    {
                        var objeto = Activator.CreateInstance(objTipoItem);
                        mapearDataAObjeto(objeto, dttDatos.Rows[i], dttDatos.Columns);
                        diccionarioMetodos["Add"].Invoke(_Objetos, new object[] { objeto });
                    }
                }
                else
                {
                    if (dttDatos.Rows.Count > 0)
                    {
                        mapearDataAObjeto(_Objetos, dttDatos.Rows[0], dttDatos.Columns);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Metodo utilizado para mapear y copiar datos de un data table
        /// </summary>
        /// <param name="_Objetos"></param>
        /// <param name="dttDatos"></param>
        public void asigarDatosDesdeDatatable(object _Objetos, DataRow[] dttDatos, System.Data.DataColumnCollection xColumnas)
        {
            try
            {
                /*OBTIENE PROPIEDADES DEL LISTADO (NOMBRE PROPIEDAD Y TIPO DE DATO) (MAPEO)*/
                /*Lista*/
                if (_Objetos.GetType().IsGenericType)
                {
                    /*OBTIENE PROPIEDADES DEL LISTADO (NOMBRE PROPIEDAD Y TIPO DE DATO) (MAPEO)*/
                    Dictionary<string, MethodInfo> diccionarioMetodos = GetDiccionarioMetodos(_Objetos);
                    var objItemTemporal = diccionarioMetodos["get_Item"];
                    Type objTipoItem = objItemTemporal.ReturnType;
                    for (var i = 0; i < dttDatos.Count(); i++)
                    {
                        var objeto = Activator.CreateInstance(objTipoItem);
                        mapearDataAObjeto(objeto, dttDatos[i], xColumnas);
                        diccionarioMetodos["Add"].Invoke(_Objetos, new object[] { objeto });
                    }
                }
                else
                {
                    if (dttDatos.Count() > 0)
                    {
                        mapearDataAObjeto(_Objetos, dttDatos[0], xColumnas);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Metodo utilizado para mapear objetos
        /// </summary>
        /// <param name="_Objetos">Ojbeto vacio</param>
        /// <param name="xFila">Fila del ojbeto</param>
        /// <param name="xColumnas">Columnas del data table</param>
        private void mapearDataAObjeto(object _Objetos, DataRow xFila, System.Data.DataColumnCollection xColumnas)
        {
            /*OBTIENE PROPIEDADES DEL LISTADO (NOMBRE PROPIEDAD Y TIPO DE DATO) (MAPEO)*/
            Dictionary<string, MethodInfo> diccionarioMetodos = GetDiccionarioMetodos(_Objetos);
            Dictionary<string, PropertyInfo> diccionarioPropiedades = new Dictionary<string, PropertyInfo>();
            string setPropiedad = "";
            foreach (DataColumn dttColumna in xColumnas)
            {
                setPropiedad = "set_" + dttColumna.ColumnName;
                if (diccionarioMetodos.ContainsKey(setPropiedad))
                {
                    diccionarioMetodos[setPropiedad].Invoke(_Objetos, new object[] { xFila[dttColumna.ColumnName] });
                }
            }
        }


        /// <summary>
        /// Metodo utilizado para mapear objetos
        /// </summary>
        /// <param name="_Objetos">Ojbeto vacio</param>
        /// <param name="xFila">Fila del ojbeto</param>
        /// <param name="xColumnas">Columnas del data table</param>
        public List<string> validaPropiedadesObjeto(object _Objetos, List<validacionObjetos> xlstValidaciones, string xPadre)
        {
            /*OBTIENE PROPIEDADES DEL LISTADO (NOMBRE PROPIEDAD Y TIPO DE DATO) (MAPEO)*/
            List<string> lstValidaciones = new List<string>();
            Dictionary<string, MethodInfo> diccionarioMetodos = GetDiccionarioMetodos(_Objetos);
            Dictionary<string, PropertyInfo> diccionarioPropiedades = new Dictionary<string, PropertyInfo>();
            string setPropiedad = "";
            List<validacionObjetos> lstValidacionesPadre = xlstValidaciones.FindAll(m => m.PropiedadRequerida_Padre == xPadre);
            foreach (validacionObjetos dttColumna in lstValidacionesPadre)
            {
                setPropiedad = "get_" + dttColumna.PropiedadRequerida;
                if (diccionarioMetodos.ContainsKey(setPropiedad))
                {
                    var objDato = diccionarioMetodos[setPropiedad].Invoke(_Objetos, new object[] { });
                    if (objDato == null) objDato = "";
                    if (dttColumna.ValorNoContenido.Contains("," + Convert.ToString(objDato) + ","))
                    {
                        lstValidaciones.Add(dttColumna.MensajeValidacion);
                    }
                    if (string.IsNullOrEmpty(dttColumna.ValorContenido) == false && dttColumna.ValorContenido.Contains("," + Convert.ToString(objDato) + ",") == false)
                    {
                        lstValidaciones.Add(dttColumna.MensajeValidacion);
                    }
                }
            }
            return lstValidaciones;
        }

        /// <summary>
        /// Metodo utilizado para retornar valores
        /// </summary>
        /// <param name="xPropiedad"></param>
        /// <param name="xObjeto"></param>
        /// <returns></returns>
        public static object retornaValor(string xPropiedad, object xObjeto)
        {
            if (!string.IsNullOrEmpty(xPropiedad))
            {
                Dictionary<string, MethodInfo> diccionarioMetodos = GetDiccionarioMetodos(xObjeto);
                string setPropiedad = "get_" + xPropiedad;
                if (diccionarioMetodos.ContainsKey(setPropiedad))
                {
                    return diccionarioMetodos[setPropiedad].Invoke(xObjeto, null);
                }
            }
            return string.Empty;
        }


    }
}