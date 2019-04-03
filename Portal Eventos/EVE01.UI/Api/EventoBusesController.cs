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
    public class EventoBusesController : ApiController
    {
        [HttpPost]
        public Respuesta<List<EventoBus>> informacionBuses()
        {
            EventoBus informacion = new EventoBus();
            Respuesta<List<EventoBus>> respuesta = informacion.informacionBuses();
            return respuesta;
        }

    }
}
