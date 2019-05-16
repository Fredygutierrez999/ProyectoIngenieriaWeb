using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProyectoCartera.Models.AccesoADatos;
using ProyectoCartera.Models.ModeloClases.Seguridad;
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

            if (string.IsNullOrEmpty(objUsuarios.Usuario))
            {
                lstErrores.Add("Debe ingresar el usuario de ingreso al sistema");
            }
            if (string.IsNullOrEmpty(objUsuarios.Nombre))
            {
                lstErrores.Add("Debe ingresar el nombre del usuario");
            }
            if (string.IsNullOrEmpty(objUsuarios.Clave))
            {
                lstErrores.Add("Debe ingresar la contraseña.");
            }
            if (string.IsNullOrEmpty(objUsuarios.CorreoElectronico))
            {
                lstErrores.Add("Debe ingresar el cooreo electrónico");
            }
            return lstErrores;
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
                    this.Comando = "pa_AppNetUsuarios_Guardar";
                    this.AgregarParametro("@ID", objUsuario.ID);
                    this.AgregarParametro("@Usuario", objUsuario.Usuario);
                    //this.AgregarParametro("@Nombre", objUsuario.@Nombre);
                    //this.AgregarParametro("@Imagen", objUsuario.Imagen);
                    //this.AgregarParametro("@Correo", objUsuario.Correo);
                    //this.AgregarParametro("@Contrasena", objUsuario.Contrasena);
                    //this.AgregarParametro("@IDAppNetCatalogo_Valores_Estado", objUsuario.IDAppNetCatalogo_Valores_Estado);
                    //this.AgregarParametro("@Json", objUsuario.Json);
                    resultado objResultado = this.TablaResultado();
                    if (objResultado.ResultadoProceso)
                    {
                        DataTable dttUsuario = objResultado.Data;
                        this.asigarDatosDesdeDatatable(objUsuario, dttUsuario);
                        objResultado.CadenaError = objUsuario.Nombre;
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
        public resultadoObjetos ConsultaUsuariosXID(int xID)
        {
            resultadoObjetos objResData = new resultadoObjetos();
            try
            {
                List<Usuarios> lstUsuario = new List<Usuarios>();
                this.Comando = "pa_AppNetUsuarios_ConsultaXid";
                this.AgregarParametro("@IDAppNetUsuarios", xID);
                resultado objResultado = this.TablaSetResultado();
                if (objResultado.ResultadoProceso)
                {
                    DataTable dttUsuarios = objResultado.DataSet.Tables[0];
                    //DataTable dttRoles = objResultado.DataSet.Tables[1];
                    //DataTable dttPermisos = objResultado.DataSet.Tables[2];
                    //DataTable dttMenus = objResultado.DataSet.Tables[3];

                    Usuarios objUsuarios = new Usuarios();
                    this.asigarDatosDesdeDatatable(objUsuarios, dttUsuarios);
                    //this.asigarDatosDesdeDatatable(objUsuarios.lstRoles, dttRoles);
                    //this.asigarDatosDesdeDatatable(objUsuarios.Permisos, dttPermisos);
                    //this.asigarDatosDesdeDatatable(objUsuarios.Menus, dttMenus);

                    lstUsuario.Add(objUsuarios);
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
        #endregion


        #region "VALIDAR SESIÓN"
        /// <summary>
        /// Metodo utilizado para validar datos del usuario
        /// </summary>
        /// <param name="objUsuario"></param>
        private List<string> ValidaDatosUsuario(Usuarios objUsuario)
        {
            List<string> lstErrores = new List<string>();
            if (string.IsNullOrEmpty(objUsuario.Usuario))
            {
                lstErrores.Add("Debe ingresar el usuario.");
            }
            if (string.IsNullOrEmpty(objUsuario.Clave))
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
        public resultadoObjetos ValidarUsuario(Usuarios objUsuario, string strUbicacion)
        {
            resultadoObjetos objResData = new resultadoObjetos();
            try
            {
                List<string> lstValidacion = this.ValidaDatosUsuario(objUsuario);
                if (lstValidacion.Count == 0)
                {
                    this.Comando = "pa_AppNetUsuarios_Validar";
                    this.AgregarParametro("@Usuario", objUsuario.Usuario);
                    this.AgregarParametro("@Contrasena", objUsuario.Clave);
                    resultado objResultado = this.TablaResultado();
                    if (objResultado.ResultadoProceso)
                    {
                        if (this.ConDatos)
                        {
                            this.asigarDatosDesdeDatatable(objUsuario, objResultado.Data);
                            objResData = this.ConsultaUsuariosXID(objUsuario.ID);
                            if (objResData.ResultadoProceso)
                            {
                                Usuarios objUsuarioSalida = ((List<Usuarios>)objResData.objetoData)[0];
                                GenerarHTMLMenu(objUsuarioSalida, strUbicacion);
                            }
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


        /// <summary>
        /// Carga listado de acciones al usuario
        /// </summary>
        /// <param name="objUsuario"></param>
        private void GenerarHTMLMenu(Usuarios objUsuario, string strUbicacionServer)
        {
            //StringBuilder objHTML = new StringBuilder();

            //List<AppNetMenu> lstMenu = objUsuario.Menus.FindAll(delegate (AppNetMenu objMenuPadre) { return objMenuPadre.IDPadre == 0; });
            //objHTML.Append("<ul class=\"navbar-nav mr-auto\">");
            //for (int i = 0; i < lstMenu.Count(); i++)
            //{
            //    List<AppNetMenu> lstMenuItems = objUsuario.Menus.FindAll(delegate (AppNetMenu objMenuPadre) { return objMenuPadre.IDPadre == lstMenu[i].ID; });
            //    objHTML.Append("<li class=\"nav-item active dropdown\">");
            //    objHTML.Append("<a class=\"nav-link text-black-50 float-right p-1 " + (lstMenuItems.Count == 0 ? "" : "dropdown-toggle") + "\" href=\"" + (lstMenuItems.Count == 0 ? lstMenu[i].Enlace + "\"" : lstMenu[i].Enlace + "\" id=\"navbarDropdown" + lstMenu[i].ID.ToString() + "\" role=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" ") + "\" >");
            //    if (!string.IsNullOrEmpty(lstMenu[i].Imagen))
            //    {
            //        objHTML.Append("<span class=\"fa icono-menu " + lstMenu[i].Imagen + " float-left pt-1 mr-1\"></span>");
            //    }
            //    objHTML.Append(lstMenu[i].Nombre + "</a>");
            //    if (lstMenuItems.Count() > 0)
            //    {
            //        objHTML.Append("<div class=\"dropdown-menu\" aria-labelledby=\"navbarDropdownMenuLink" + lstMenu[i].ID.ToString() + "\">");
            //        for (int j = 0; j < lstMenuItems.Count(); j++)
            //        {
            //            int xtipoMenu = lstMenuItems[j].IDTipoMenu;
            //            switch (xtipoMenu)
            //            {
            //                case 1: /*OPCION  MENU*/
            //                    objHTML.Append("<a href=\"" + lstMenuItems[j].Enlace + "\" class=\"dropdown-item ml-3\">");
            //                    if (!string.IsNullOrEmpty(lstMenuItems[j].Imagen))
            //                    {
            //                        objHTML.Append("<span class=\"fa icono-menu " + lstMenuItems[j].Imagen + " float-left pt-1 mr-1\"></span>");
            //                    }
            //                    objHTML.Append(lstMenuItems[j].Nombre + "</a>");
            //                    break;
            //                case 2: /*SEPARADOR*/
            //                    objHTML.Append("<div class=\"dropdown-divider\"></div>");
            //                    break;
            //                case 3: /*ENCABEZADO*/
            //                    objHTML.Append("<a href=\"" + lstMenuItems[j].Enlace + "\" class=\"dropdown-item disabled\" style=\"font-size: 10px;\">");
            //                    if (!string.IsNullOrEmpty(lstMenuItems[j].Imagen))
            //                    {
            //                        objHTML.Append("<span class=\"fa icono-menu " + lstMenuItems[j].Imagen + " float-left pt-1 mr-1\"></span>");
            //                    }
            //                    objHTML.Append(lstMenuItems[j].Nombre + " </a>");
            //                    break;
            //            }
            //        }
            //        objHTML.Append("</div>");
            //    }
            //    objHTML.Append("</li>");
            //}
            //objHTML.Append("</ul>");

            //if (objHTML.Length > 0)
            //{
            //    string capetaMenu = Path.Combine(strUbicacionServer, "MenuUsuario");
            //    if (!Directory.Exists(capetaMenu))
            //    {
            //        Directory.CreateDirectory(capetaMenu);
            //    }
            //    /*DEBE EXISTIR LA RUTA*/
            //    if (Directory.Exists(capetaMenu))
            //    {
            //        string strNombreCarpeta = "Menu_" + objUsuario.ID.ToString() + ".html";
            //        string strRutaCompleta = Path.Combine(capetaMenu, strNombreCarpeta);
            //        if (File.Exists(strRutaCompleta))
            //        {
            //            File.Delete(strRutaCompleta);
            //        }
            //        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            //        byte[] byteCadena = encoding.GetBytes(objHTML.ToString());
            //        FileStream strArchivo = File.Create(strRutaCompleta, byteCadena.Length, FileOptions.None);
            //        strArchivo.Write(byteCadena, 0, byteCadena.Length);
            //        strArchivo.Close();
            //        strArchivo.Dispose();

            //        /*ASIGA CADENA A OBJETO SE SESSION*/
            //        objUsuario.CadenaMenuUsuario = strRutaCompleta;
            //    }
            //}
        }
        #endregion


    }
}