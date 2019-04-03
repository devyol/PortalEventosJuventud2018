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
    public class InscripcionBusController : ApiController
    {
        [HttpPost]
        public Respuesta<List<EventoBus>> busesDisponiblesInscrito([FromBody] Inscripcion obj)
        {
            InscripcionBus disponible = new InscripcionBus();
            Respuesta<List<EventoBus>> respuesta = disponible.busesDisponiblesParticipante(obj);
            return respuesta;
        }

        [HttpPost]
        public Respuesta<InscripcionBus> asignacionBus([FromBody] InscripcionBus obj)
        {
            return obj.asignacionBus();
        }

        [HttpPost]
        public Respuesta<InscripcionBus> infoBusAsignado([FromBody] Participante obj)
        {
            InscripcionBus informacion = new InscripcionBus();
            Respuesta<InscripcionBus> respuesta = informacion.infoBusAsignado(obj);
            return respuesta;
        }
    }
}
