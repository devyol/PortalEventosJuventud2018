using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVE01.UI.Clases
{
    public class Respuesta<T>
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public string mensajeError { get; set; }
        public int CantidadRegistros { get; set; }
        public T data { get; set; }
    }
}