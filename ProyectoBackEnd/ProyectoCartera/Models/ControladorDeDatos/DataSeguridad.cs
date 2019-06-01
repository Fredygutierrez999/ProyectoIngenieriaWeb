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
    /// Clase utilizda para administrar datos del modilo de seguridad (usuarios, Roles, Inicio sesion etc)
    /// Creado por: FAGV
    /// Fecha creación: 11/28/2018
    /// </summary>
    public class DataSeguridad : Datos
    {


        #region "USUARIOS"

        /// <summary>
        /// Metod utilizado para consultar usuarios
        /// </summary>
        /// <param name="xUsuario">Filtro Usuario</param>
        /// <param name="xNombre">Filtro Nombre</param>
        /// <param name="xCorreo">Filtro Correo</param>
        /// <returns></returns>
        public resultadoObjetos ConsultaUsuarios(int xId, string xUsuario, string xNombre, string xCorreo)
        {
            resultadoObjetos objResData = new resultadoObjetos();
            try
            {
                List<Usuarios> lstUsuario = new List<Usuarios>();
                this.Comando = "pa_AppNetUsuarios_Consulta";
                this.AgregarParametro("@ID", Datos.NullAValDefecto(xId));
                this.AgregarParametro("@Usuario", Datos.NullAValDefecto(xUsuario));
                this.AgregarParametro("@Nombre", Datos.NullAValDefecto(xNombre));
                this.AgregarParametro("@Correo", Datos.NullAValDefecto(xCorreo));
                resultado objResultado = this.TablaSetResultado();
                if (objResultado.ResultadoProceso)
                {
                    DataTable dttUsuarios = objResultado.DataSet.Tables[0];
                    DataTable dttRoles = objResultado.DataSet.Tables[1];
                    DataTable dttPermisos = objResultado.DataSet.Tables[2];

                    for (int i = 0; i < dttUsuarios.Rows.Count; i++)
                    {
                        Usuarios objUsuarios = new Usuarios();
                        this.asigarDatosDesdeDatatable(objUsuarios, new DataRow[] { dttUsuarios.Rows[i] }, dttUsuarios.Columns);

                        //DataRow[] dtrRoles = dttRoles.Select(" IDAppNetUsuarios = " + objUsuarios.ID.ToString());
                        //this.asigarDatosDesdeDatatable(objUsuarios.lstRoles, dtrRoles, dttRoles.Columns);

                        //DataRow[] dtrpermisos = dttPermisos.Select(" IDAppNetUsuarios = " + objUsuarios.ID.ToString());
                        //this.asigarDatosDesdeDatatable(objUsuarios.Permisos, dtrpermisos, dttPermisos.Columns);

                        lstUsuario.Add(objUsuarios);
                    }
                }

                objResData.set(objResultado);
                objResData.objetoData = lstUsuario;
            }
            catch (Exception ex)
            {
                objResData.cargarError(ex);
            }
            return objResData;
        }

        /// <summary>
        /// Metodo utilizado para validar datos del usuario
        /// </summary>
        /// <param name="objUsuarios">Objeto de usuario</param>
        /// <returns></returns>
        private List<string> validaDatosUsuario(Usuarios objUsuarios)
        {
            List<string> lstErrores = new List<string>();
            if (objUsuarios.identificacion == 0)
            {
                lstErrores.Add("Debe ingresar el número de identificación.");
            }
            else
            {
                if (validaExistenciaUsuario(objUsuarios.identificacion).ResultadoProceso)
                {
                    lstErrores.Add("El usuario ya existe.");
                }
            }
            if (string.IsNullOrEmpty(objUsuarios.tipo_identificacion))
            {
                lstErrores.Add("Debe seleccionar un tipo de identificación");
            }
            if (string.IsNullOrEmpty(objUsuarios.Nombre))
            {
                lstErrores.Add("Debe ingresar el nombre del usuario");
            }
            if (string.IsNullOrEmpty(objUsuarios.Apellido))
            {
                lstErrores.Add("Debe ingresar el apellido del usuario");
            }
            if (string.IsNullOrEmpty(objUsuarios.Contrasena))
            {
                lstErrores.Add("Debe ingresar la contraseña.");
            }
            if (objUsuarios.Contrasena != objUsuarios.Contrasena_Transaccion)
            {
                lstErrores.Add("La confirmación de la clave no es la misma.");
            }
            if (string.IsNullOrEmpty(objUsuarios.email))
            {
                lstErrores.Add("Debe ingresar el correo electrónico");
            }
            if (objUsuarios.fecha_nacimiento == DateTime.MinValue)
            {
                lstErrores.Add("Debe índicar una fecha de nacimiento valida");
            }
            if (objUsuarios.AceptaTerminos == false)
            {
                lstErrores.Add("Debe aceptar términos y condiciones");
            }
            return lstErrores;
        }

        /// <summary>
        /// Metodo utilizado para validar existencia de usuario
        /// </summary>
        /// <param name="xIdentificacion"></param>
        /// <returns></returns>
        public resultadoObjetos validaExistenciaUsuario(long xIdentificacion)
        {
            resultadoObjetos objResData = new resultadoObjetos();
            try
            {
                this.Comando = "pa_Usuarios_Administrar_Validacion";
                this.AgregarParametro("@identificacion", xIdentificacion);
                resultado objResultado = this.TablaResultado();
                if (objResultado.ResultadoProceso)
                {
                    DataTable dttUsuario = objResultado.Data;
                    objResData.ResultadoProceso = dttUsuario.Rows.Count > 0;
                }
                objResData.set(objResultado);
            }
            catch (Exception ex)
            {
                objResData.cargarError(ex);
            }
            return objResData;
        }

        /// <summary>
        /// Metodo utilizado para gudardar datos del usuario
        /// </summary>
        /// <param name="xNombre">Filtro Nombre</param>
        /// <returns></returns>
        public resultadoObjetos UsuariosGuardar(Usuarios objUsuario)
        {
            resultadoObjetos objResData = new resultadoObjetos();
            try
            {
                List<string> lstUsuario = validaDatosUsuario(objUsuario);
                if (lstUsuario.Count == 0)
                {
                    this.Comando = "pa_Usuarios_Administrar";
                    this.AgregarParametro("@identificacion", objUsuario.identificacion);
                    this.AgregarParametro("@tipo_identificacion", objUsuario.tipo_identificacion);
                    this.AgregarParametro("@Nombre_Usuario", objUsuario.Nombre_Usuario);
                    this.AgregarParametro("@Contrasena", objUsuario.Contrasena);
                    this.AgregarParametro("@Contrasena_Transaccion", objUsuario.Contrasena_Transaccion);
                    this.AgregarParametro("@Nombre", objUsuario.Nombre);
                    this.AgregarParametro("@Apellido", objUsuario.Apellido);
                    this.AgregarParametro("@genero", objUsuario.genero);
                    this.AgregarParametro("@email", objUsuario.email);
                    this.AgregarParametro("@fecha_nacimiento", objUsuario.fecha_nacimiento);
                    resultado objResultado = this.TablaResultado();
                    if (objResultado.ResultadoProceso)
                    {
                        DataTable dttUsuario = objResultado.Data;
                        this.asigarDatosDesdeDatatable(objResultado, dttUsuario);
                    }
                    objResData.set(objResultado);
                    objResData.objetoData = objUsuario;
                }
                else
                {
                    objResData.CadenaError = objResData.generarHMLTValidaciones(lstUsuario);
                    objResData.setTipoMensaje(resultadoObjetos.EnumTipoMensaje.ADVERTENCIA);
                    objResData.ResultadoProceso = false;
                }
            }
            catch (Exception ex)
            {
                objResData.cargarError(ex);
            }
            return objResData;
        }

        /// <summary>
        /// Metodo utilizado para consultar usuarios por ID
        /// </summary>
        /// <param name="xID"></param>
        /// <returns></returns>
        public resultadoObjetos ConsultaUsuariosXNombre(string xNombreUsuario)
        {
            resultadoObjetos objResData = new resultadoObjetos();
            try
            {
                Usuarios objUsuarios = new Usuarios();
                this.Comando = "pa_Usuarios_ConsultaUsuario";
                this.AgregarParametro("@Nombre_Usuario", xNombreUsuario);
                resultado objResultado = this.TablaSetResultado();
                if (objResultado.ResultadoProceso)
                {
                    DataTable dttUsuarios = objResultado.DataSet.Tables[0];
                    this.asigarDatosDesdeDatatable(objUsuarios, dttUsuarios);
                }

                objResData.set(objResultado);
                objResData.objetoData = objUsuarios;
            }
            catch (Exception ex)
            {
                objResData.cargarError(ex);
            }
            return objResData;
        }

        #endregion


        #region "VALIDAR SESIÓN"

        /// <summary>
        /// Metodo utilizado para validar datos del usuario
        /// </summary>
        /// <param name="objUsuario"></param>
        private List<string> ValidaDatosUsuario(Usuarios objUsuario)
        {
            List<string> lstErrores = new List<string>();
            if (string.IsNullOrEmpty(objUsuario.Nombre_Usuario))
            {
                lstErrores.Add("Debe ingresar el usuario.");
            }
            if (string.IsNullOrEmpty(objUsuario.Contrasena))
            {
                lstErrores.Add("Debe ingresar la clave.");
            }
            return lstErrores;
        }

        /// <summary>
        /// Metodo utilizado para validar usuario
        /// </summary>
        /// <param name="objUsuario">Objeto usuario</param>
        /// <returns></returns>
        public resultadoObjetos ValidarUsuario(Usuarios objUsuario)
        {
            resultadoObjetos objResData = new resultadoObjetos();
            try
            {
                List<string> lstValidacion = this.ValidaDatosUsuario(objUsuario);
                if (lstValidacion.Count == 0)
                {
                    this.Comando = "pa_Usuarios_ValidaUsuario";
                    this.AgregarParametro("@Nombre_Usuario", objUsuario.Nombre_Usuario);
                    this.AgregarParametro("@Contrasena", objUsuario.Contrasena);
                    resultado objResultado = this.TablaResultado();
                    if (objResultado.ResultadoProceso)
                    {
                        if (this.ConDatos)
                        {
                            this.asigarDatosDesdeDatatable(objUsuario, objResultado.Data);
                        }
                        else
                        {
                            objResData.ResultadoProceso = false;
                            objResData.CadenaError = "El usuario no existe";
                        }
                    }
                    else
                    {
                        objResData.set(objResultado);
                    }
                }
                else
                {
                    objResData.ResultadoProceso = false;
                    objResData.CadenaError = lstValidacion[0];
                }
            }
            catch (Exception ex)
            {
                objResData.cargarError(ex);
            }
            return objResData;
        }

        #endregion

    }
}