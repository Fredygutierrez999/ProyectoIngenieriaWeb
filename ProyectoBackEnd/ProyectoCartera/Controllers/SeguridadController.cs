using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using ProyectoCartera.App_Start;
using ProyectoCartera.Models.ModeloClases.Seguridad;
using ProyectoCartera.Models.ControladorDeDatos;
using ProyectoCartera.Models.ModeloClases;
using ProyectoCartera.Models.AccesoADatos;

namespace ProyectoCartera.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/Seguridad")]
    public class SeguridadController : ApiController
    {
        private DataSeguridad objDataSeguridad;

        /// <summary>
        /// Constructor
        /// </summary>
        public SeguridadController()
        {
            this.objDataSeguridad = new DataSeguridad();
        }

        /// <summary>
        /// Realiza prueba de conexión con el api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }


        /// <summary>
        /// Indica si el usuario se encuentra registrado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }


        /// <summary>
        /// Autentica el usuario por nombre y clave
        /// </summary>
        /// <param name="login">Objeto usuario</param>
        /// <returns></returns>
        [HttpGet]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(string Nombre_Usuario, string Contrasena)
        {
            ResultadoJSON _resultado = null;
            try
            {
                Usuarios login = new Usuarios() { Nombre_Usuario = Nombre_Usuario, Contrasena = Contrasena };
                if (login == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                ///Valida usuario en la BD
                var _respuesta = this.objDataSeguridad.ValidarUsuario(login);
                if (_respuesta.ResultadoProceso)
                {
                    var token = TokenGenerator.GenerateTokenJwt(login.Nombre_Usuario, login.Tipo_Usuario.ToString(), login.identificacion.ToString());
                    _resultado = new ResultadoJSON() { ResultadoProceso = true, objetoData = token };
                }
                else
                {
                    _resultado = new ResultadoJSON() { ResultadoProceso = false, CadenaError = _resultado.CadenaError };
                }
            }
            catch (Exception ex)
            {
                _resultado = new ResultadoJSON() { ResultadoProceso = false, CadenaError = ex.Message };
            }
            return Json(_resultado);
        }



        /// <summary>
        /// Autentica el usuario por nombre y clave
        /// </summary>
        /// <param name="login">Objeto usuario</param>
        /// <returns></returns>
        [HttpGet]
        [Route("registrar")]
        public IHttpActionResult registrar(
            int identificacion,
            string tipo_identificacion,
            string Nombre_Usuario,
            string Contrasena,
            string Contrasena_Transaccion,
            string Nombre,
            string Apellido,
            string genero,
            string email,
            string fecha_nacimiento
        )
        {
            ResultadoJSON _resultado = null;
            try
            {
                Usuarios login = new Usuarios()
                {
                    identificacion = identificacion,
                    tipo_identificacion = tipo_identificacion,
                    Nombre_Usuario = email,
                    Contrasena = Contrasena,
                    Contrasena_Transaccion = Contrasena_Transaccion,
                    Nombre = Nombre,
                    Apellido = Apellido,
                    genero = genero,
                    email = email,
                    fecha_nacimiento = Convert.ToDateTime(fecha_nacimiento)
                };

                ///Valida usuario en la BD
                var _respuesta = this.objDataSeguridad.ValidarUsuario(login);
                if (_respuesta.ResultadoProceso)
                {
                    var token = TokenGenerator.GenerateTokenJwt(login.Nombre_Usuario, login.Tipo_Usuario.ToString(),login.identificacion.ToString());
                    _resultado = new ResultadoJSON() { ResultadoProceso = true, objetoData = token };
                }
                else
                {
                    _resultado = new ResultadoJSON() { ResultadoProceso = false, CadenaError = _resultado.CadenaError };
                }

            }
            catch (Exception ex)
            {
                _resultado = new ResultadoJSON() { ResultadoProceso = false, CadenaError = ex.Message };
            }
            return Json(_resultado);
        }

    }
}