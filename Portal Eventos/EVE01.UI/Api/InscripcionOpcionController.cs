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
    public class InscripcionOpcionController : ApiController
    {
        [HttpPost]
        public Respuesta<List<InscripcionOpcion>> listarOpcionesInscripcion([FromBody]Participante insc)
        {
            InscripcionOpcion obj = new InscripcionOpcion();
            Respuesta<List<InscripcionOpcion>> res = obj.opcionesDeInscripcion(insc);
            return res;
        }

        [HttpPost]
        public Respuesta<InscripcionOpcion> cambiarEstadoOpcion([FromBody]InscripcionOpcion insc)
        {
            return insc.modificarEstadoOpcion();
        }
    }
}
