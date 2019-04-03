using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Transactions;
using EVE01.UI.Clases;
using EVE01.DO.DATA;

namespace EVE01.UI.Models
{
    public class Recibo
    {
        #region Atributos Privados

        private EVE01_RECIBO dbModel;        

        #endregion

        #region Constantes

        public const string _sqlinfoRecibo = @"select
                                                rec.recibo idRecibo,
                                                rec.cantidad,
                                                rec.nombre nombreCompleto,
                                                rec.cantidad_letras cantidadLetras,
                                                eve.nombre_evento concepto,
                                                det.descripcion opcion,
                                                det.precio precioopcion,
                                                decode(rec.no_bus,0,'SIN BUS',to_char(rec.no_bus)) noBusAsignado,
                                                evebus.hora_salida,
                                                silla.no_silla noSilla,
                                                saldo.total,
                                                saldo.saldo_abonado,
                                                saldo.saldo_pendiente,
                                                rec.usuario_creacion usuarioCreacion,
                                                to_char(rec.fecha_creacion, 'dd/mm/yyyy hh24:mi:ss') fechaEmision
                                                from
                                                eve01_recibo rec,
                                                eve01_inscripcion_silla silla,
                                                eve01_inscripcion_bus bus,
                                                eve01_evento_bus evebus,
                                                eve01_participante_saldo saldo,
                                                eve01_evento eve,
                                                eve01_recibo_detalle det
                                                where rec.evento = eve.evento
                                                and rec.evento = silla.evento(+)
                                                and rec.participante = silla.participante(+)
                                                and rec.evento = bus.evento(+)
                                                and rec.participante = bus.participante(+)
                                                and bus.evento = evebus.evento(+)
                                                and bus.bus = evebus.bus(+)
                                                and rec.evento = saldo.evento
                                                and rec.participante = saldo.participante
                                                and rec.recibo = det.recibo
                                                and rec.movimiento = :movimiento";

        #endregion

        #region Propiedades Publicas

        public decimal idRecibo
        {
            get { return dbModel.RECIBO; }
            set { dbModel.RECIBO = value; }
        }

        public decimal? idMovimiento
        {
            get { return dbModel.MOVIMIENTO; }
            set { dbModel.MOVIMIENTO = value; }
        }

        public decimal? idParticipante
        {
            get { return dbModel.PARTICIPANTE; }
            set { dbModel.PARTICIPANTE = value; }
        }

        public decimal? idEvento
        {
            get { return dbModel.EVENTO; }
            set { dbModel.EVENTO = value; }
        }

        public decimal? noBus
        {
            get { return dbModel.NO_BUS; }
            set { dbModel.NO_BUS = value; }
        }

        public string nombreCompleto
        {
            get { return dbModel.NOMBRE; }
            set { dbModel.NOMBRE = value; }
        }

        public decimal? cantidad
        {
            get { return dbModel.CANTIDAD; }
            set { dbModel.CANTIDAD = value; }
        }

        public string cantidadLetras
        {
            get { return dbModel.CANTIDAD_LETRAS; }
            set { dbModel.CANTIDAD_LETRAS = value; }
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

        public string concepto { get; set; }
        public string opcion { get; set; }
        public decimal precioopcion { get; set; }
        public string noBusAsignado { get; set; }
        public decimal? noSilla { get; set; }
        public decimal total { get; set; }
        public decimal saldo_abonado { get; set; }
        public decimal saldo_pendiente { get; set; }
        public string fechaEmision { get; set; }
        public string hora_salida { get; set; }

        #endregion        

        #region Constructores

        public Recibo()
        {
            dbModel = new EVE01_RECIBO();
        }

        public Recibo(EVE01_RECIBO datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<Recibo> nuevoRecibo()
        {
            Respuesta<Recibo> result = new Respuesta<Recibo>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de Datos";
            result.data = new Recibo();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var correlativo = (from r in db.EVE01_RECIBO
                                      select r.RECIBO).Max();

                        var corrRecibo = correlativo + 1;

                        correlativo = correlativo == null ? 1 : corrRecibo;

                        EVE01_RECIBO nuevo = new EVE01_RECIBO();
                        nuevo.RECIBO = correlativo;
                        nuevo.MOVIMIENTO = this.idMovimiento;
                        nuevo.PARTICIPANTE = this.idParticipante;//
                        nuevo.EVENTO = MvcApplication.idEvento;
                        nuevo.NO_BUS = this.noBus;//
                        nuevo.NOMBRE = this.nombreCompleto;//
                        nuevo.CANTIDAD = this.cantidad;//
                        nuevo.CANTIDAD_LETRAS = NumeroLetras.NumeroALetras(this.cantidad.ToString());
                        nuevo.ESTADO_REGISTRO = "A";
                        nuevo.USUARIO_CREACION = MvcApplication.UserName;
                        nuevo.FECHA_CREACION = DateTime.Now;
                        db.EVE01_RECIBO.Add(nuevo);
                        int rnr = db.SaveChanges();

                        if (rnr <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "No fue posible registrar el recibo";
                            return result;
                        }

                        //REGISTRAR EL DETALLE DEL RECIBO
                        ReciboDetalle detalle = new ReciboDetalle();
                        detalle.idRecibo = correlativo;
                        detalle.idParticipante = this.idParticipante;
                        Respuesta<ReciboDetalle> respuesta = detalle.guardarDetalle();

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
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al tratar de registrar el recibo, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public List<Recibo> impresionRecibo()
        {
            List<Recibo> result = new List<Recibo>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    StringBuilder strSQL = new StringBuilder();
                    strSQL.Append(_sqlinfoRecibo);

                    var info = db.Database.SqlQuery<Recibo>(strSQL.ToString(), new object[] { this.idMovimiento }).ToList<Recibo>();
                    
                    if (info != null)
                    {
                        result = info;
                    }
                    else
                    {
                        result = new List<Recibo>();
                    }
                }
                return result;

            }
            catch (Exception)
            {                
                throw;
            }
        }

        #endregion

        #region Metodos Privados
        #endregion
    }
}