using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EVE01.UI.Clases;
using EVE01.UI.Models;

namespace EVE01.UI.Api
{
    public class EstadisticaController : ApiController
    {
        [HttpPost]
        public Respuesta<List<Estadisticas>> listaValores()
        {
            Estadisticas objEstadistica = new Estadisticas();
            Respuesta<List<Estadisticas>> res = objEstadistica.Valores();
            return res;
        }

        [HttpPost]
        public Respuesta<List<Estadisticas>> listaTotales()
        {
            Estadisticas objEstadistica = new Estadisticas();
            Respuesta<List<Estadisticas>> res = objEstadistica.Totales();
            return res;
        }

    }
}
