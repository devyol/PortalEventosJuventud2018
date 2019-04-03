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
    public class InscripcionHospedajeController : ApiController
    {

        [HttpPost]
        public Respuesta<InscripcionHospedaje> anotacionHospedaje([FromBody]InscripcionHospedaje obj)
        {
            return obj.registrarHospedaje();
        }

        

        [HttpPost]
        public Respuesta<InscripcionHospedaje> infoHospedaje([FromBody]InscripcionHospedaje obj)
        {
            return obj.datosHospedaje();
        }

    }
}
