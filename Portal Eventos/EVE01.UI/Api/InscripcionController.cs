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
    public class InscripcionController : ApiController
    {
        [HttpPost]
        public Respuesta<Inscripcion> Inscribir([FromBody]Inscripcion insc)
        {
            return insc.Inscribir();
        }

        [HttpPost]
        public Respuesta<Inscripcion> Anular([FromBody]Inscripcion insc)
        {
            return insc.AnularInscripcion();
        }
    }
}
