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
    public class ParticipanteController : ApiController
    {
        [HttpPost]
        public Respuesta<List<Participante>> ObtenerListadoParticipantes()
        {
            Participante obj = new Participante();
            return obj.listaParticipatnesActivos();
        }

        [HttpPost]
        public Respuesta<Participante> ObtenerParticipante([FromBody]Participante obj)
        {
            try
            {
                return obj.obtenerInformacionParticipante();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        [HttpPost]
        public Respuesta<Participante> NuevoParticipante([FromBody]Participante obj)
        {
            return obj.Nuevo();
        }

        [HttpPost]
        public Respuesta<Participante> ActualizarParticipante([FromBody]Participante obj)
        {
            return obj.Actualizar();
        }

    }
}
