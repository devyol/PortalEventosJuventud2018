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
    public class EventosController : ApiController
    {
        [HttpPost]
        public Respuesta<List<Evento>> ListadoEventosActivos()
        {
            Evento objEvento = new Evento();
            Respuesta<List<Evento>> result = objEvento.ListarEventosActivos();
            return result;
        }

        [HttpPost]
        public Respuesta<List<Evento>> ListadoEventos()
        {
            Respuesta<List<Evento>> ObjRespuesta = new Respuesta<List<Evento>>();
            Evento ObjEvento = new Evento();
            ObjRespuesta = ObjEvento.ListadoEventos();
            return ObjRespuesta;
        }

        [HttpPost]
        public Respuesta<Evento> ValidaEventoActivo()
        {
            Evento objEvento = new Evento();
            Respuesta<Evento> result = objEvento.validaEventoActivo();
            return result;
        }

        [HttpPost]
        public Respuesta<Evento> AsignarEventoGlobal([FromBody] Evento obj)
        {
            return obj.SetEventoActivo();
        }
    }
}
