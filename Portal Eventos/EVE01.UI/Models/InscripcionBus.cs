using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.UI.Clases;
using EVE01.DO.DATA;
using System.Transactions;


namespace EVE01.UI.Models
{
    public class InscripcionBus
    {
        #region Atributos Privados

        private EVE01_INSCRIPCION_BUS dbModel;        

        #endregion

        #region Propiedades Publicas

        public decimal idBus
        {
            get { return dbModel.BUS; }
            set { dbModel.BUS = value; }
        }

        public decimal idParticipante
        {
            get { return dbModel.PARTICIPANTE; }
            set { dbModel.PARTICIPANTE = value; }
        }

        public decimal idEvento
        {
            get { return dbModel.EVENTO; }
            set { dbModel.EVENTO = value; }
        }

        public Nullable<decimal> noBus
        {
            get { return dbModel.NO_BUS; }
            set { dbModel.NO_BUS = value; }
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

        public bool validaRadio { get; set; }
        public string numeroBus { get; set; }

        #endregion

        #region Constructores

        public InscripcionBus()
        {
            dbModel = new EVE01_INSCRIPCION_BUS();
        }

        public InscripcionBus(EVE01_INSCRIPCION_BUS datos)
        {
            dbModel = new EVE01_INSCRIPCION_BUS();
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<List<EventoBus>> busesDisponiblesParticipante(Inscripcion data)
        {
            Respuesta<List<EventoBus>> result = new Respuesta<List<EventoBus>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en base de Datos";
            result.data = new List<EventoBus>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var buses = (from b in db.EVE01_EVENTO_BUS
                                 where b.EVENTO == MvcApplication.idEvento
                                 && b.ESTADO_REGISTRO == "A"
                                 select b).ToList();

                    if (buses.Count == 0)
                    {
                        result.codigo = -1;
                        result.mensaje = "No Existen Buses registrados para este Evento";
                        return result;
                    }
                    else
                    {
                        foreach (var item in buses)
                        {
                            if (item.DISPONIBLE != 0 || validaAsignacion(data.idParticipante, item.BUS)) //REVISA SI HAY LUGARES DISPONIBLES PARA LUEGO MOSTRAR EN INTERFACE GRAFICA
                            {
                                result.data.Add(new EventoBus(item) { validabusasignado = validaAsignacion(data.idParticipante, item.BUS) });
                            }
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
                result.mensaje = "Ocurrio una excepcion al obtener el listado de buses disponibles, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }


        }

        public Respuesta<InscripcionBus> asignacionBus()
        {
            Respuesta<InscripcionBus> result = new Respuesta<InscripcionBus>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en base de datos";
            result.data = new InscripcionBus();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        if (this.validaRadio == false)
                        {
                            var validaDisponibles = (from vd in db.EVE01_EVENTO_BUS
                                                     where vd.EVENTO == MvcApplication.idEvento
                                                     && vd.BUS == this.idBus
                                                     select vd).SingleOrDefault();
                            
                            if (validaDisponibles.DISPONIBLE != 0)
                            {
                                var tieneBus = (from tb in db.EVE01_INSCRIPCION_BUS
                                                where tb.PARTICIPANTE == this.idParticipante
                                                && tb.EVENTO == MvcApplication.idEvento
                                                select tb).SingleOrDefault();

                                if (tieneBus != null)//EL PARTICIPANTE YA TIENE BUS ASIGNADO POR LO QUE HAY QUE MODIFICAR
                                {
                                    //SE ACTUALIZA INFORMACION DE BUSES
                                    InscripcionBus actualizaBuses = new InscripcionBus();
                                    actualizaBuses.idParticipante = this.idParticipante;
                                    actualizaBuses.idBus = this.idBus;
                                    Respuesta<InscripcionBus> respuesta = actualizaBuses.actualizarInformacionBuses();

                                    if (respuesta.codigo != 0)
                                    {
                                        Transaction.Current.Rollback();
                                        result.codigo = respuesta.codigo;
                                        result.mensaje = respuesta.mensaje;
                                        return result;
                                    }
                                    //SE ELIMINA EL REGISTRO DEL BUS ASIGNADO ANTERIOR
                                    var eliminaBusAnterior = (from eba in db.EVE01_INSCRIPCION_BUS
                                                              where eba.EVENTO == tieneBus.EVENTO
                                                              && eba.BUS == tieneBus.BUS
                                                              && eba.PARTICIPANTE == tieneBus.PARTICIPANTE
                                                              select eba).SingleOrDefault();

                                    db.EVE01_INSCRIPCION_BUS.Remove(eliminaBusAnterior);
                                    int ce = db.SaveChanges();

                                    if (ce <= 0)
                                    {
                                        Transaction.Current.Rollback();
                                        result.codigo = -2;
                                        result.mensaje = "No fue posible eliminar el bus anterior";
                                        return result;
                                    }

                                    EVE01_INSCRIPCION_BUS nuevoBus = new EVE01_INSCRIPCION_BUS();
                                    nuevoBus.BUS = this.idBus;
                                    nuevoBus.PARTICIPANTE = this.idParticipante;
                                    nuevoBus.EVENTO = MvcApplication.idEvento;
                                    nuevoBus.NO_BUS = this.noBus;
                                    nuevoBus.ESTADO_REGISTRO = "A";
                                    nuevoBus.USUARIO_CREACION = MvcApplication.UserName;
                                    nuevoBus.FECHA_CREACION = DateTime.Now;
                                    db.EVE01_INSCRIPCION_BUS.Add(nuevoBus);
                                    int ri = db.SaveChanges();

                                    if (ri <= 0)
                                    {
                                        Transaction.Current.Rollback();
                                        result.codigo = -2;
                                        result.mensaje = "No fue posible agregar el bus actual";
                                        return result;                                       
                                    }
                                }
                                else//NO TIENE BUS ASIGNA POR LO QUE SE PROCEDE A INSERTAR EL BUS
                                {

                                    //SE ACTUALIZA INFORMACION DE BUSES
                                    InscripcionBus actualizaBuses = new InscripcionBus();
                                    actualizaBuses.idParticipante = this.idParticipante;
                                    actualizaBuses.idBus = this.idBus;
                                    Respuesta<InscripcionBus> respuesta = actualizaBuses.actualizarInformacionBuses();

                                    if (respuesta.codigo != 0)
                                    {
                                        Transaction.Current.Rollback();
                                        result.codigo = respuesta.codigo;
                                        result.mensaje = respuesta.mensaje;
                                        return result;
                                    }

                                    EVE01_INSCRIPCION_BUS asignaBus = new EVE01_INSCRIPCION_BUS();
                                    asignaBus.BUS = this.idBus;
                                    asignaBus.PARTICIPANTE = this.idParticipante;
                                    asignaBus.EVENTO = MvcApplication.idEvento;
                                    asignaBus.NO_BUS = this.noBus;
                                    asignaBus.ESTADO_REGISTRO = "A";
                                    asignaBus.USUARIO_CREACION = MvcApplication.UserName;
                                    asignaBus.FECHA_CREACION = DateTime.Now;
                                    db.EVE01_INSCRIPCION_BUS.Add(asignaBus);
                                    int rin = db.SaveChanges();

                                    if (rin <= 0)
                                    {
                                        Transaction.Current.Rollback();
                                        result.codigo = -2;
                                        result.mensaje = "No fue posible asignar el Bus";
                                        return result;
                                    }
                                }
                            }
                            else
                            {
                                result.codigo = -1;
                                result.mensaje = "Ya no hay lugares disponibles, para este bus con No. " + validaDisponibles.NO_BUS;
                                return result;
                            }
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
                result.mensaje = "Ocurrio una excepcion al tratar de asignar bus, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<InscripcionBus> infoBusAsignado(Participante data)
        {
            Respuesta<InscripcionBus> result = new Respuesta<InscripcionBus>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de Datos";
            result.data = new InscripcionBus();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var info = (from b in db.EVE01_INSCRIPCION_BUS
                                where b.EVENTO == MvcApplication.idEvento
                                && b.PARTICIPANTE == data.idParticipante
                                select b).SingleOrDefault();
                    if (info != null)
                    {
                        dbModel = info;
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
                result.mensaje ="Ocurrio una excepcion al tratar de consultar el No de Bus, ref; "+ ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }


        #endregion

        #region Metodos Privados

        private bool validaAsignacion(decimal pa, decimal bus)
        {
            using (var db = new EntitiesEVE01())
            {
                var infobus = (from i in db.EVE01_INSCRIPCION_BUS
                               where i.EVENTO == MvcApplication.idEvento
                               && i.PARTICIPANTE == pa
                               && i.BUS == bus
                               select i).SingleOrDefault();

                if (infobus == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }        

        private Respuesta<InscripcionBus> actualizarInformacionBuses()
        {
            Respuesta<InscripcionBus> result = new Respuesta<InscripcionBus>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en base de datos";
            result.data = new InscripcionBus();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var busAsignado = (from ba in db.EVE01_INSCRIPCION_BUS
                                            where ba.EVENTO == MvcApplication.idEvento
                                            && ba.PARTICIPANTE == this.idParticipante
                                            select ba).SingleOrDefault();

                        if (busAsignado != null)
                        {
                            var busAnterior = (from ban in db.EVE01_EVENTO_BUS
                                               where ban.EVENTO == MvcApplication.idEvento
                                               && ban.BUS == busAsignado.BUS
                                               select ban).SingleOrDefault();

                            busAnterior.DISPONIBLE++;
                            busAnterior.OCUPADO--;
                            busAnterior.USUARIO_MODIFICACION = MvcApplication.UserName;
                            busAnterior.FECHA_MODIFICACION = DateTime.Now;
                            int rba = db.SaveChanges();

                            if (rba <= 0)
                            {
                                Transaction.Current.Rollback();
                                result.codigo = -2;
                                result.mensaje = "No fue posible Actualizar Informacion el Bus Anterior";
                                return result;
                            }

                            var busActual = (from ba in db.EVE01_EVENTO_BUS
                                             where ba.EVENTO == MvcApplication.idEvento
                                             && ba.BUS == this.idBus
                                             select ba).SingleOrDefault();

                            busActual.DISPONIBLE--;
                            busActual.OCUPADO++;
                            busActual.USUARIO_MODIFICACION = MvcApplication.UserName;
                            busActual.FECHA_MODIFICACION = DateTime.Now;
                            int raba = db.SaveChanges();

                            if (raba <= 0)
                            {
                                Transaction.Current.Rollback();
                                result.codigo = -2;
                                result.mensaje = "No fue posible Actualizar Informacion del Bus Actual";
                                return result;                                
                            }
                        }
                        else
                        {
                            var busActual = (from ba in db.EVE01_EVENTO_BUS
                                             where ba.EVENTO == MvcApplication.idEvento
                                             && ba.BUS == this.idBus
                                             select ba).SingleOrDefault();

                            busActual.DISPONIBLE--;
                            busActual.OCUPADO++;
                            busActual.USUARIO_MODIFICACION = MvcApplication.UserName;
                            busActual.FECHA_MODIFICACION = DateTime.Now;
                            int raba = db.SaveChanges();

                            if (raba <= 0)
                            {
                                Transaction.Current.Rollback();
                                result.codigo = -2;
                                result.mensaje = "No fue posible Actualizar Informacion del Bus Actual";
                                return result;
                            }
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
                result.mensaje = "Ocurrio una excepcion al actualizar informacion de Busese, ref: " + ex.ToString();
                return result;
            }
        }

        #endregion

    }
}