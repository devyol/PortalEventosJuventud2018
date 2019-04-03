using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;


namespace EVE01.UI.Models
{
    public class SaldoParticipante
    {
        #region Atributos Privados

        private EVE01_PARTICIPANTE_SALDO dbModel;        

        #endregion

        #region Propiedades Publicas

        public decimal idEvento
        {
            get { return dbModel.EVENTO; }
            set { dbModel.EVENTO = value; }
        }

        public decimal idParticipante
        {
            get { return dbModel.PARTICIPANTE; }
            set { dbModel.PARTICIPANTE = value; }
        }

        public Nullable<decimal> total
        {
            get { return dbModel.TOTAL; }
            set { dbModel.TOTAL = value; }
        }

        public Nullable<decimal> saldoAbonado
        {
            get { return dbModel.SALDO_ABONADO; }
            set { dbModel.SALDO_ABONADO = value; }
        }

        public Nullable<decimal> saldoPendiente
        {
            get { return dbModel.SALDO_PENDIENTE; }
            set { dbModel.SALDO_PENDIENTE = value; }
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

        public Nullable<DateTime> fechaCreacion
        {
            get { return dbModel.FECHA_CREACION; }
            set { dbModel.FECHA_CREACION = value; }
        }

        public Nullable<DateTime> fechaModificacion
        {
            get { return dbModel.FECHA_MODIFICACION; }
            set { dbModel.FECHA_MODIFICACION = value; }
        }


        #endregion

        #region Constructores

        public SaldoParticipante()
        {
            dbModel = new EVE01_PARTICIPANTE_SALDO();
        }

        public SaldoParticipante(EVE01_PARTICIPANTE_SALDO datos)
        {
            dbModel = datos;
        }
        #endregion

        #region Metodos Publicos

        public Respuesta<SaldoParticipante> saldo(Participante data)
        {
            Respuesta<SaldoParticipante> result = new Respuesta<SaldoParticipante>();
            result.codigo = 0;
            result.mensaje = "Ocurrio un Error en base de datos";
            result.data = new SaldoParticipante();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var saldos = (from s in db.EVE01_PARTICIPANTE_SALDO
                                  where s.EVENTO == MvcApplication.idEvento
                                  && s.PARTICIPANTE == data.idParticipante
                                  select s).SingleOrDefault();

                    if (saldos == null)
                    {
                        result.codigo = -1;
                        result.mensaje = "No Hay Saldos para este Participante";
                        return result;
                    }
                    else
                    {
                        dbModel = saldos;
                    }                    
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                result.data = this;
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al obtener los Saldos, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<SaldoParticipante> registrarSaldoInicial(Inscripcion data)
        {
            Respuesta<SaldoParticipante> result = new Respuesta<SaldoParticipante>();
            result.codigo = 0;
            result.mensaje = "Ocurrio un Error en base de datos";
            result.data = new SaldoParticipante();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    //SE REALIZA LA SUMATORIA DEL PRECIO DE LAS OPCIONES OBLIGATORIAS 
                    var total = (from io in db.EVE01_INSCRIPCION_OPCION
                                    join oe in db.EVE01_EVENTO_OPCION
                                    on new { io.EVENTO, io.OPCION } equals new {oe.EVENTO, oe.OPCION }
                                    where io.EVENTO == MvcApplication.idEvento
                                    && io.PARTICIPANTE == data.idParticipante
                                    && io.ESTADO_REGISTRO == "A"
                                    select oe.PRECIO).Sum();

                    //Y SE INSERTAN EN LA TABLA DE SALDOS
                    if (total > 0)
                    {
                        EVE01_PARTICIPANTE_SALDO nuevo = new EVE01_PARTICIPANTE_SALDO();
                        nuevo.EVENTO = MvcApplication.idEvento;
                        nuevo.PARTICIPANTE = data.idParticipante;
                        nuevo.TOTAL = total;
                        nuevo.SALDO_ABONADO = 0;
                        nuevo.SALDO_PENDIENTE = total;
                        nuevo.ESTADO_REGISTRO = "A";
                        nuevo.USUARIO_CREACION = MvcApplication.UserName.ToUpper();
                        nuevo.FECHA_CREACION = DateTime.Now;

                        db.EVE01_PARTICIPANTE_SALDO.Add(nuevo);
                        db.SaveChanges();
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";                
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al registrar el Saldo Inicial, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<SaldoParticipante> anulacionSaldos(Inscripcion data)
        {
            Respuesta<SaldoParticipante> result = new Respuesta<SaldoParticipante>();
            result.codigo = 0;
            result.mensaje = "Ocurrio un Error en base de datos";
            result.data = new SaldoParticipante();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    //SE OBTINE EL REGISTRO PARA SU ELIMINACION
                    var saldos = (from s in db.EVE01_PARTICIPANTE_SALDO
                                  where s.EVENTO == MvcApplication.idEvento
                                  && s.PARTICIPANTE == data.idParticipante
                                  select s).SingleOrDefault();

                    db.EVE01_PARTICIPANTE_SALDO.Remove(saldos);
                    db.SaveChanges();
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al eliminar el Saldo Inicial, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<SaldoParticipante> actualizarSaldos(InscripcionOpcion data)
        {
            Respuesta<SaldoParticipante> result = new Respuesta<SaldoParticipante>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de datos";
            result.data = new SaldoParticipante();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var total = (from io in db.EVE01_INSCRIPCION_OPCION
                                 join oe in db.EVE01_EVENTO_OPCION
                                 on new { io.EVENTO, io.OPCION } equals new { oe.EVENTO, oe.OPCION }
                                 where io.EVENTO == MvcApplication.idEvento
                                 && io.PARTICIPANTE == data.idParticipante
                                 && io.ESTADO_REGISTRO == "A"
                                 select oe.PRECIO).Sum();

                    var Abonos = (from s in db.EVE01_MOVIMIENTO
                                  where s.EVENTO == MvcApplication.idEvento
                                  && s.PARTICIPANTE == data.idParticipante
                                  && s.TIPO_MOVIMIENTO == "AB"
                                  select s.CANTIDAD).Sum();

                    Abonos = Abonos == null ? 0 : Abonos;

                    var Cargos = (from s in db.EVE01_MOVIMIENTO
                                  where s.EVENTO == MvcApplication.idEvento
                                  && s.PARTICIPANTE == data.idParticipante
                                  && s.TIPO_MOVIMIENTO == "CR"
                                  select s.CANTIDAD).Sum();

                    Cargos = Cargos == null ? 0 : Cargos;

                    var saldoAbonado = Abonos - Cargos; //CUANDO YA SE TENGA LA TABLA DE MOVIMIENTO, DE AHI SE OBTIENE EL SALDO ABONADO
                    var saldoPendiente = total - saldoAbonado;

                    var saldo = (from s in db.EVE01_PARTICIPANTE_SALDO
                                 where s.EVENTO == MvcApplication.idEvento
                                 && s.PARTICIPANTE == data.idParticipante
                                 select s).SingleOrDefault();

                    saldo.TOTAL = total;
                    saldo.SALDO_ABONADO = saldoAbonado;
                    saldo.SALDO_PENDIENTE = saldoPendiente;
                    saldo.USUARIO_MODIFICACION = MvcApplication.UserName.ToUpper();
                    saldo.FECHA_MODIFICACION = DateTime.Now;
                    db.SaveChanges();                    
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion al actualizar los Saldos, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }        

        #endregion
    }
}