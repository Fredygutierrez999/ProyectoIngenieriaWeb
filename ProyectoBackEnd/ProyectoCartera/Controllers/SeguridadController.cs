using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using ProyectoCartera.App_Start;
using ProyectoCartera.Models.ModeloClases.Seguridad;


namespace ProyectoCartera.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class SeguridadController : ApiController
    {

        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(Usuarios login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //TODO: This code is only for demo - extract method in new class & validate correctly in your application !!
            var isUserValid = (login.Usuario == "user" && login.Clave == "123456");
            if (isUserValid)
            {
                var rolename = "Developer";
                var token = TokenGenerator.GenerateTokenJwt(login.Usuario, rolename);
                return Ok(token);
            }

            //TODO: This code is only for demo - extract method in new class & validate correctly in your application !!
            var isTesterValid = (login.Usuario == "test" && login.Clave == "123456");
            if (isTesterValid)
            {
                var rolename = "Tester";
                var token = TokenGenerator.GenerateTokenJwt(login.Usuario, rolename);
                return Ok(token);
            }

            //TODO: This code is only for demo - extract method in new class & validate correctly in your application !!
            var isAdminValid = (login.Usuario == "admin" && login.Clave == "123456");
            if (isAdminValid)
            {
                var rolename = "Administrator";
                var token = TokenGenerator.GenerateTokenJwt(login.Usuario, rolename);
                return Ok(token);
            }

            // Unauthorized access 
            return Unauthorized();
        }

    }
}