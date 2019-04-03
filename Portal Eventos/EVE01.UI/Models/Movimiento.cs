using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using EVE01.DO.DATA;
using EVE01.UI.Clases;
using System.Text;

namespace EVE01.UI.Models
{
    public class Movimiento
    {

        #region Constantes

        private const string _sqlSaldosDiarios = @"select
                                                    to_char(fecha,'dd/mm/yyyy') fechasaldo,
                                                    total saldodiario
                                                    from(
                                                    select trunc(fecha_creacion) fecha, sum(cantidad) total
                                                    from eve01_movimiento
                                                    where tipo_movimiento = 'AB'
                                                    and evento = :evento
                                                    group by trunc(fecha_creacion)
                                                    order by trunc(fecha_creacion) desc)";

        private const string _sqlSaldoEvento = @"select ev.nombre_evento, 
                                                    count(participante)inscritos, 
                                                    sum(total)total, 
                                                    sum(saldo_abonado)abonado, 
                                                    sum(saldo_pendiente)pendiente
                                                    from eve01_participante_saldo sp, eve01_evento ev
                                                    where sp.evento = ev.evento
                                                    and sp.evento = :evento
                                                    group by ev.nombre_evento";
        #endregion

        #region Atributos Privados

        private EVE01_MOVIMIENTO dbModel;        

        #endregion

        #region Propiedades Publicas

        public decimal idMovimiento
        {
            get { return dbModel.MOVIMIENTO; }
            set { dbModel.MOVIMIENTO = value; }
        }

        public decimal idParticipante
        {
            get { return dbModel.PARTICIPANTE; }
            set { dbModel.PARTICIPANTE = value; }
        }

        public decimal? idEvento
        {
            get { return dbModel.EVENTO; }
            set { dbModel.EVENTO = value; }
        }

        public string idTipoMovimiento
        {
            get { return dbModel.TIPO_MOVIMIENTO; }
            set { dbModel.TIPO_MOVIMIENTO = value; }
        }

        public decimal? idTipoPago
        {
            get { return dbModel.TIPO_PAGO; }
            set { dbModel.TIPO_PAGO = value; }
        }

        public decimal? cantidad
        {
            get { return dbModel.CANTIDAD; }
            set { dbModel.CANTIDAD = value; }
        }

        public string estadoRegistro
        {
            get { return dbModel.ESTADO_REGISTRO; }
            set { dbModel.ESTADO_REGISTRO = value; }
        }

        public string usuarioCreacion
        {
            get { return dbModel.USUARIO_CREACION; }
            set { dbModel.USUARIO_CREACION = value; }
        }

        public string usuarioModificacion
        {
            get { return dbModel.USUARIO_MODIFICACION; }
            set { dbModel.USUARIO_MODIFICACION = value; }
        }

        public DateTime? fechaCreacion
        {
            get { return dbModel.FECHA_CREACION; }
            set { dbModel.FECHA_CREACION = value; }
        }

        public DateTime? fechaModificacion
        {
            get { return dbModel.FECHA_MODIFICACION; }
            set { dbModel.FECHA_MODIFICACION = value; }
        }

        public decimal? noBus { get; set; }

        public string nombreCompleto { get; set; }

        public string descripcionTipoPago { get; set; }

        public string descripcionTipoMovimiento { get; set; }

        public string fechasaldo { get; set; }

        public decimal? saldodiario { get; set; }

        public string nombre_evento { get; set; }

        public decimal? inscritos { get; set; }

        public decimal? total { get; set; }

        public decimal? abonado { get; set; }

        public decimal? pendiente { get; set; }

        #endregion

        #region Constructores

        public Movimiento()
        {
            dbModel = new EVE01_MOVIMIENTO();
        }

        public Movimiento(EVE01_MOVIMIENTO datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<Movimiento> guardarMovimiento()
        {
            Respuesta<Movimiento> result = new Respuesta<Movimiento>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de Datos";
            result.data = new Movimiento();

            if (this.cantidad <= 0 || this.cantidad == null)
            {
                result.codigo = -1;
                result.mensaje = "No es posible realizar el cobro el valor no puede ser negativo, 0 o vacio, escriba una cantidad valida";
                return result;
            }

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        //SE VALIDA QUE EL MONTO QUE COBRA NO EXEDA EL SALDO PENDIENTE
                        var validaSaldoRestante = (from sr in db.EVE01_PARTICIPANTE_SALDO
                                                   where sr.EVENTO == MvcApplication.idEvento
                                                   && sr.PARTICIPANTE == this.idParticipante
                                                   select sr).SingleOrDefault();

                        if (this.cantidad > validaSaldoRestante.SALDO_PENDIENTE)
                        {
                            result.codigo = -1;
                            result.mensaje = "No es posible realizar un cobro mayor al Saldo Pendiente o un valor negativo, favor de revisar el monto a cobrar";
                            return result;
                        }

                        //SE VALIDA SI EL PARTICIPANTE TIENE BUS
                        var validaOpcionBus = (from oe in db.EVE01_EVENTO_OPCION
                                               join oi in db.EVE01_INSCRIPCION_OPCION
                                               on new { oe.EVENTO, oe.OPCION } equals new { oi.EVENTO, oi.OPCION }
                                               where oe.ES_TRANSPORTE == "S"
                                               && oi.EVENTO == MvcApplication.idEvento
                                               && oi.PARTICIPANTE == this.idParticipante
                                               && oi.ESTADO_REGISTRO == "A"
                                               select oi).SingleOrDefault();

                        var validaBusAsignado = (from ba in db.EVE01_INSCRIPCION_BUS
                                                 where ba.EVENTO == MvcApplication.idEvento
                                                 && ba.PARTICIPANTE == this.idParticipante
                                                 select ba).SingleOrDefault();

                        if (validaOpcionBus != null && validaBusAsignado == null)
                        {
                            result.codigo = -1;
                            result.mensaje = "EL PARTICIPANTE TIENE LA OPCION DE BUS PERO NO TIENE BUS ASIGNADO, FAVOR DE ASIGNAR BUS";
                            return result;
                        }

                        //VALIDA SI EL EVENTO LLEVA CONTROL DE SILLAS
                        var validasillaEvento = (from vs in db.EVE01_EVENTO_SILLA
                                                 where vs.EVENTO == MvcApplication.idEvento
                                                 select vs).SingleOrDefault();

                        if (validasillaEvento != null)
                        {
                            var validasillaasignada = (from sa in db.EVE01_INSCRIPCION_SILLA
                                                       where sa.EVENTO == MvcApplication.idEvento
                                                       && sa.PARTICIPANTE == this.idParticipante
                                                       select sa).SingleOrDefault();

                            var saldoAbonadoParticipante = validaSaldoRestante.SALDO_ABONADO + this.cantidad;

                            //VALIDA QUE NO TENGA SILLA ASIGNADA Y SI EL MONTO ES MAYOR O IGUAL AL MONTO MINIMO PARA ASIGNAR SILLA
                            if (validasillaasignada == null && saldoAbonadoParticipante >= validasillaEvento.SALDO_MINIMO)
                            {
                                var silla = (from s in db.EVE01_INSCRIPCION_SILLA
                                             where s.EVENTO == MvcApplication.idEvento                                             
                                             select s.NO_SILLA).Max();

                                var sillaasignada = silla + 1;

                                silla = silla == null ? 1 : sillaasignada;

                                EVE01_INSCRIPCION_SILLA nuevasilla = new EVE01_INSCRIPCION_SILLA();
                                nuevasilla.EVENTO = MvcApplication.idEvento;
                                nuevasilla.PARTICIPANTE = this.idParticipante;
                                nuevasilla.NO_SILLA = silla;
                                nuevasilla.ESTADO_REGISTRO = "A";
                                nuevasilla.USUARIO_CREACION = MvcApplication.UserName;
                                nuevasilla.FECHA_CREACION = DateTime.Now;
                                db.EVE01_INSCRIPCION_SILLA.Add(nuevasilla);

                                int vrs = db.SaveChanges();

                                if (vrs <= 0)
                                {
                                    result.codigo = -2;
                                    result.mensaje = "No fue posible registrar el Cobro, inconveniente en la asignacion de Silla";
                                    return result;
                                }
                            }
                        }

                        //SE PROCEDE A REGISTRAR EL MOVIMIENTO DEL COBRO
                        var correlativo = (from c in db.EVE01_MOVIMIENTO
                                            select c.MOVIMIENTO).Max();

                        var corrMov = correlativo + 1;

                        correlativo = correlativo == null ? 1 : corrMov;

                        EVE01_MOVIMIENTO nuevo = new EVE01_MOVIMIENTO();
                        nuevo.MOVIMIENTO = correlativo;
                        nuevo.PARTICIPANTE = this.idParticipante;
                        nuevo.EVENTO = MvcApplication.idEvento;
                        nuevo.TIPO_MOVIMIENTO = "AB";
                        nuevo.TIPO_PAGO = this.idTipoPago;
                        nuevo.CANTIDAD = this.cantidad;
                        nuevo.ESTADO_REGISTRO = "A";
                        nuevo.USUARIO_CREACION = MvcApplication.UserName;
                        nuevo.FECHA_CREACION = DateTime.Now;
                        db.EVE01_MOVIMIENTO.Add(nuevo);
                        int rnm = db.SaveChanges();

                        if (rnm <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "No fue posible registrar el Movimiento";
                            return result;
                        }

                        //ACTUALIZAR SALDOS POR EL PAGO REALIZADO
                        var saldo = (from s in db.EVE01_PARTICIPANTE_SALDO
                                        where s.EVENTO == MvcApplication.idEvento
                                        && s.PARTICIPANTE == this.idParticipante
                                        select s).SingleOrDefault();

                        saldo.SALDO_ABONADO = saldo.SALDO_ABONADO + this.cantidad;
                        saldo.SALDO_PENDIENTE = saldo.SALDO_PENDIENTE - this.cantidad;
                        saldo.USUARIO_MODIFICACION = MvcApplication.UserName;
                        saldo.FECHA_MODIFICACION = DateTime.Now;
                        int ras = db.SaveChanges();

                        if (ras <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "No fue posible registrar el Movimiento, ocurrio un inconveniente al actualizar saldos";
                            return result;
                        }


                        var nBus = validaBusAsignado == null ? 0 : validaBusAsignado.NO_BUS;

                        //REGISTRAR EL RECIBO
                        Recibo nuevoRecibo = new Recibo();
                        nuevoRecibo.idMovimiento = correlativo;
                        nuevoRecibo.idParticipante = this.idParticipante;
                        nuevoRecibo.noBus = nBus;
                        nuevoRecibo.nombreCompleto = this.nombreCompleto;
                        nuevoRecibo.cantidad = this.cantidad;
                        Respuesta<Recibo> respuesta = nuevoRecibo.nuevoRecibo();

                        if (respuesta.codigo != 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = respuesta.codigo;
                            result.mensaje = respuesta.mensaje;
                            return result;
                        }
                    }
                    tr.Complete();
                }
                result.codigo = 0;
                result.mensaje = "Se registro Correctamente el Pago por: " + this.cantidad;
                return result;

            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al tratar de registrar el Movimiento, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        //METODO PARA ANULAR MOVIMIENTOS CUANDO SE ANULA INSCRIPCION
        public Respuesta<Movimiento> anularMovimientos(decimal participante)
        {
            Respuesta<Movimiento> result = new Respuesta<Movimiento>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en base de datos";
            result.data = new Movimiento();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var movimientos = (from m in db.EVE01_MOVIMIENTO
                                       where m.EVENTO == MvcApplication.idEvento
                                       && m.PARTICIPANTE == participante
                                       && m.TIPO_MOVIMIENTO == "AB"
                                       select m).ToList();

                    if (movimientos.Count > 0)
                    {
                        foreach (var item in movimientos)
                        {
                            var mov = (from mo in db.EVE01_MOVIMIENTO
                                       where mo.MOVIMIENTO == item.MOVIMIENTO
                                       && mo.EVENTO == item.EVENTO
                                       && mo.PARTICIPANTE == item.PARTICIPANTE
                                       select mo).SingleOrDefault();

                            mov.TIPO_MOVIMIENTO = "AN";
                            mov.USUARIO_MODIFICACION = MvcApplication.UserName;
                            mov.FECHA_MODIFICACION = DateTime.Now;                            
                            db.SaveChanges();
                        }
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al momento de anular los movimientos, ref: " + ex.ToString();
                return result;
            }


        }

        public Respuesta<List<Movimiento>> listaMovimientos()
        {
            Respuesta<List<Movimiento>> result = new Respuesta<List<Movimiento>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de datos";
            result.data = new List<Movimiento>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var movimientos = (from mov in db.EVE01_MOVIMIENTO
                                       orderby mov.MOVIMIENTO descending
                                       where mov.EVENTO == MvcApplication.idEvento
                                       && mov.PARTICIPANTE == this.idParticipante
                                       && mov.TIPO_MOVIMIENTO == "AB"
                                       select mov).ToList();

                    if (movimientos.Count > 0)
                    {
                        foreach (var item in movimientos)
                        {
                            result.data.Add(new Movimiento(item) 
                            {
                                descripcionTipoPago = infodescripcionTipoPago(item.TIPO_PAGO),
                                descripcionTipoMovimiento = infodescripcionTipoMovimiento(item.TIPO_MOVIMIENTO)
                            });
                        }
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al tratar de obtener los movimientos, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<List<Movimiento>> listaMovimientosCargos()
        {
            Respuesta<List<Movimiento>> result = new Respuesta<List<Movimiento>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de datos";
            result.data = new List<Movimiento>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var movimientos = (from mov in db.EVE01_MOVIMIENTO
                                       orderby mov.MOVIMIENTO descending
                                       where mov.EVENTO == MvcApplication.idEvento
                                       && mov.PARTICIPANTE == this.idParticipante
                                       && mov.TIPO_MOVIMIENTO == "CR"
                                       select mov).ToList();

                    if (movimientos.Count > 0)
                    {
                        foreach (var item in movimientos)
                        {
                            result.data.Add(new Movimiento(item)
                            {
                                descripcionTipoPago = infodescripcionTipoPago(item.TIPO_PAGO),
                                descripcionTipoMovimiento = infodescripcionTipoMovimiento(item.TIPO_MOVIMIENTO)
                            });
                        }
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al tratar de obtener los movimientos, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<List<Movimiento>> listaMovimientosAnulados()
        {
            Respuesta<List<Movimiento>> result = new Respuesta<List<Movimiento>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de datos";
            result.data = new List<Movimiento>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var movimientos = (from mov in db.EVE01_MOVIMIENTO
                                       orderby mov.MOVIMIENTO descending
                                       where mov.EVENTO == MvcApplication.idEvento
                                       && mov.PARTICIPANTE == this.idParticipante
                                       && mov.TIPO_MOVIMIENTO == "AN"
                                       select mov).ToList();

                    if (movimientos.Count > 0)
                    {
                        foreach (var item in movimientos)
                        {
                            result.data.Add(new Movimiento(item)
                            {
                                descripcionTipoPago = infodescripcionTipoPago(item.TIPO_PAGO),
                                descripcionTipoMovimiento = infodescripcionTipoMovimiento(item.TIPO_MOVIMIENTO)
                            });
                        }
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al tratar de obtener los movimientos, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        //public Respuesta<Movimiento> anularMovimientoManual()
        //{
        //    Respuesta<Movimiento> result = new Respuesta<Movimiento>();
        //    result.codigo = 1;
        //    result.mensaje = "Ocurrio un Error en Base de Datos";
        //    result.data = new Movimiento();

        //    if (this.cantidad <= 0 || string.IsNullOrWhiteSpace(this.cantidad.ToString()))
        //    {
        //        result.codigo = -1;
        //        result.mensaje = "La cantidad no puede ser Negativa, con Valor 0 o Vacia";
        //        return result;
        //    }

        //    try
        //    {
        //        using (var tr = new TransactionScope())
        //        {
        //            using (var db = new EntitiesEVE01())
        //            {
        //                var saldo = (from s in db.EVE01_PARTICIPANTE_SALDO
        //                             where s.EVENTO == MvcApplication.idEvento
        //                             && s.PARTICIPANTE == this.idParticipante
        //                             select s).SingleOrDefault();

        //                if (saldo != null)
        //                {
        //                    //if (this.cantidad > saldo.SALDO_PENDIENTE)
        //                    //{
        //                    //    result.codigo = -1;
        //                    //    result.mensaje = "El valor de la Nota de Credito excede el Valor de saldo Pendiente, favor de revisar";
        //                    //    return result;
        //                    //}
        //                }
        //            }
        //            tr.Complete();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result.codigo = -1;
        //        result.mensaje = "Ocurrio una excepcion al realizar la anulacion del Movimiento, ref: " + ex.ToString();
        //        result.mensajeError = ex.ToString();
        //        return result;
        //    }

        //    return result;
        //}

        public Respuesta<List<Movimiento>> saldosDiariosEvento()
        {
            Respuesta<List<Movimiento>> result = new Respuesta<List<Movimiento>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de Datos";
            result.data = new List<Movimiento>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    StringBuilder sqlSaldos = new StringBuilder();
                    sqlSaldos.Append(_sqlSaldosDiarios);

                    var res = db.Database.SqlQuery<Movimiento>(sqlSaldos.ToString(), new object[] { MvcApplication.idEvento }).ToList<Movimiento>();

                    if (res.Count > 0)
                    {
                        result.data = res;
                    }
                    else
                    {
                        result.data = new List<Movimiento>();
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al obtener el listado de Saldos, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<Movimiento> saldoTotalEvento()
        {
            Respuesta<Movimiento> result = new Respuesta<Movimiento>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de Datos";
            result.data = new Movimiento();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    StringBuilder sqlSaldoEvento = new StringBuilder();
                    sqlSaldoEvento.Append(_sqlSaldoEvento);

                    var res = db.Database.SqlQuery<Movimiento>(sqlSaldoEvento.ToString(), new object[] { MvcApplication.idEvento }).SingleOrDefault<Movimiento>();

                    if (res != null)
                    {
                        result.data = res;
                    }
                    else
                    {
                        result.data = new Movimiento();
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al obtener el saldo del evento, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        #endregion


        #region Metodos Privados

        private string infodescripcionTipoPago(decimal? idtipo)
        {
            using (var db = new EntitiesEVE01())
            {
                var des = (from d in db.EVE01_TIPO_PAGO
                           where d.TIPO_PAGO == idtipo
                           select d.DESCRIPCION).SingleOrDefault();
                return des;
            }
        }

        private string infodescripcionTipoMovimiento(string idtipo)
        {
            using (var db = new EntitiesEVE01())
            {
                var des = (from d in db.EVE01_TIPO_MOVIMIENTO
                           where d.TIPO_MOVIMIENTO == idtipo
                           select d.DESCRIPCION).SingleOrDefault();
                return des;
            }
        }


        #endregion


    }
}