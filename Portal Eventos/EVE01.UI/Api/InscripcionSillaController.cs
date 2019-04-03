using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EVE01.UI.Clases;
using EVE01.UI.Models;
using EVE01.DO.DATA;

namespace EVE01.UI.Api
{
    public class InscripcionSillaController : ApiController
    {
        [HttpPost]
        public Respuesta<InscripcionSilla> infoSillaAsignada([FromBody] InscripcionSilla obj)
        {
            return obj.infosillaAsignada();
        }

    }
}
