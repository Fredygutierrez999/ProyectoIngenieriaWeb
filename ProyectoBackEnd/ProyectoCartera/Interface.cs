using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace ProyectoCartera
{
    public interface Interface
    {
        int Id { get; set; }
        string Nombre { get; set; }
    }
}