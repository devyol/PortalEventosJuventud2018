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
    public class ParticipanteSaldosController : ApiController
    {
        [HttpPost]
        public Respuesta<SaldoParticipante> mostrarSaldoParticipante([FromBody]Participante insc)
        {
            SaldoParticipante obj = new SaldoParticipante();
            Respuesta<SaldoParticipante> res = obj.saldo(insc);
            return res;
        }

    }
}
