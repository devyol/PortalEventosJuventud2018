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
    public class MovimientosController : ApiController
    {
        [HttpPost]
        public Respuesta<Movimiento> registraMovimiento([FromBody] Movimiento obj)
        {
            return obj.guardarMovimiento();
        }

        [HttpPost]
        public Respuesta<List<Movimiento>> listaMovimientos([FromBody] Movimiento obj)
        {
            return obj.listaMovimientos();
        }

        [HttpPost]
        public Respuesta<List<Movimiento>> listaMovimientosCargos([FromBody] Movimiento obj)
        {
            return obj.listaMovimientosCargos();
        }

        [HttpPost]
        public Respuesta<List<Movimiento>> listaMovimientosAnulados([FromBody] Movimiento obj)
        {
            return obj.listaMovimientosAnulados();
        }

        [HttpPost]
        public Respuesta<List<Movimiento>> saldosDiariosEvento()
        {
            Movimiento objSaldos = new Movimiento();
            Respuesta<List<Movimiento>> respuesta = objSaldos.saldosDiariosEvento();
            return respuesta;
        }

        [HttpPost]
        public Respuesta<Movimiento> saldoTotalEvento()
        {
            Movimiento objSaldoEvento = new Movimiento();
            Respuesta<Movimiento> respuesta = objSaldoEvento.saldoTotalEvento();
            return respuesta;
        }
    }
}
