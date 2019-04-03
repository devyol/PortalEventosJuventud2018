using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;
using System.Transactions;
using EVE01.UI.Models;
using System.Data.Objects;



namespace EVE01.UI.Models
{
    public class InscripcionOpcion
    {
        
        #region Atributos Privados

        private EVE01_INSCRIPCION_OPCION dbModel;

        #endregion

        #region Propiedades Publicas

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

        public decimal idOpcion
        {
            get { return dbModel.OPCION; }
            set { dbModel.OPCION = value; }
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

        public DateTime? fechaCreacion
        {
            get { return dbModel.FECHA_CREACION; }
            set { dbModel.FECHA_CREACION = value; }
        }

        public string usuarioModificacion
        {
            get { return dbModel.USUARIO_MODIFICACION; }
            set { dbModel.USUARIO_MODIFICACION = value; }
        }

        public DateTime? fechaModificacion
        {
            get { return dbModel.FECHA_MODIFICACION; }
            set { dbModel.FECHA_MODIFICACION = value; }
        }

        public Estado valorEstado { get; set; }
        public bool es_obligatorio { get; set; }
        public bool es_transporte { get; set; }
        public bool es_hospedaje { get; set; }
        public string descripcion { get; set; }
        public bool estadoopcion { get; set; }
        public Nullable<decimal> precio { get; set; }

        #endregion

        #region Costructores

        public InscripcionOpcion()
        {
            dbModel = new EVE01_INSCRIPCION_OPCION();
        }

        public InscripcionOpcion(EVE01_INSCRIPCION_OPCION datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<InscripcionOpcion> agregarOpcionesInscripcion(Inscripcion data)
        {
            Respuesta<InscripcionOpcion> result = new Respuesta<InscripcionOpcion>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de Datos";
            result.data = new InscripcionOpcion();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {

                        //SE OBTIENE EL LISTADO DE OPCIONES REGISTRADAS PARA EL EVENTO ACTUAL
                        var listadoOpciones = db.EVE01_EVENTO_OPCION.
                                              Where(o => o.EVENTO == MvcApplication.idEvento &&
                                                    o.ESTADO_REGISTRO == "A").
                                              Select(o => o).ToList();

                        //SE EVALUA SI EXISTEN LAS OPCIONES
                        if (listadoOpciones.Count == 0)
                        {
                            result.codigo = 1;
                            result.mensaje = "No existen Opciones para este Evento";
                            return result;
                        }
                        else
                        {
                            //SE RECORRE EL LISTADO DE LAS OPCIONES Y POR CADA UNA QUE SE ENCUENTRA SE INSERTAN EN LA TABLA EVE01_INSCRIPCION_OPCION
                            foreach (var item in listadoOpciones)
                            {
                                EVE01_INSCRIPCION_OPCION opcion = new EVE01_INSCRIPCION_OPCION();
                                opcion.PARTICIPANTE = data.idParticipante;
                                opcion.EVENTO = MvcApplication.idEvento;
                                opcion.OPCION = item.OPCION;
                                //SI LA OPCION ES OBLIGATORIA, SE REGISTRA EN ESTADO "A" DE LO CONTRARIO EN ESTADO "B"
                                if (item.OBLIGATORIO == "S")
                                {
                                    opcion.ESTADO_REGISTRO = "A";
                                }
                                else
                                {
                                    opcion.ESTADO_REGISTRO = "B";
                                }
                                opcion.USUARIO_CREACION = MvcApplication.UserName;
                                opcion.FECHA_CREACION = DateTime.Now;
                                db.EVE01_INSCRIPCION_OPCION.Add(opcion);
                                int res_op = db.SaveChanges();

                                if (res_op <= 0)
                                {
                                    Transaction.Current.Rollback();
                                    result.codigo = 2;
                                    result.mensaje = "No fue Posible Registrar las Opciones";
                                    return result;
                                }
                            }
                        }                        

                        //SE REGISTRA EL SALDO INICIAL
                        SaldoParticipante nuevo = new SaldoParticipante();
                        Respuesta<SaldoParticipante> respuesta = nuevo.registrarSaldoInicial(data);

                        if (respuesta.codigo != 0)
	                    {
                            Transaction.Current.Rollback();
                            result.codigo = respuesta.codigo;
                            result.mensaje = respuesta.mensajeError;
                            return result;
	                    }
                    }
                    tr.Complete();
                    result.codigo = 0;
                    result.mensaje = "Se agregaron correctamente las opciones del Evento";
                    return result;
                }

            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al momento de agregar las opciones del Evento, Ref:" + ex.ToString();
                return result;
            }            
        }

        //METODO QUE INACTIVA LAS OPCIONES DE INSCRIPCION AL MOMENTO DE ANULAR LA INSCRIPCION
        public Respuesta<InscripcionOpcion> eliminarOpcionesInscripcion(Inscripcion data)
        {
            Respuesta<InscripcionOpcion> result = new Respuesta<InscripcionOpcion>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de Datos";
            result.data = new InscripcionOpcion();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var listadoOpciones = db.EVE01_INSCRIPCION_OPCION.
                                              Where(o => o.PARTICIPANTE == data.idParticipante &&
                                                         o.EVENTO == MvcApplication.idEvento).
                                              Select(o => o).ToList();

                        if (listadoOpciones.Count == 0)
                        {
                            result.codigo = -1;
                            result.mensaje = "No existen Opciones para la Inscripcion indicada";
                            result.data = new InscripcionOpcion();
                            return result;
                        }
                        else
                        {
                            foreach (var item in listadoOpciones)
                            {
                                var del = db.EVE01_INSCRIPCION_OPCION.
                                          Where(d => d.PARTICIPANTE == item.PARTICIPANTE &&
                                                     d.EVENTO == item.EVENTO &&
                                                     d.OPCION == item.OPCION).
                                          Select(d => d).SingleOrDefault();

                                db.EVE01_INSCRIPCION_OPCION.Remove(del);
                                int re = db.SaveChanges();

                                if (re <= 0)
                                {
                                    Transaction.Current.Rollback();
                                    result.codigo = -2;
                                    result.mensaje = "No fue posible eliminar la opcion";
                                    return result;
                                }
                            }

                            //SI EN LAS OPCIONES TIENE ASIGNADO BUS SE PROCEDE A ELIMINAR Y ACTUALIZAR LA DISPONIBILIDAD DE BUS PARA OTRO PARTICIPANTE
                            var tienebusAsignado = (from ob in db.EVE01_INSCRIPCION_BUS
                                                     where ob.EVENTO == MvcApplication.idEvento
                                                     && ob.PARTICIPANTE == data.idParticipante
                                                     select ob).SingleOrDefault();

                            if (tienebusAsignado != null)
                            {
                                var actualizarInfoBus = (from aib in db.EVE01_EVENTO_BUS
                                                         where aib.EVENTO == tienebusAsignado.EVENTO
                                                         && aib.BUS == tienebusAsignado.BUS
                                                         select aib).SingleOrDefault();

                                actualizarInfoBus.DISPONIBLE++;
                                actualizarInfoBus.OCUPADO--;
                                actualizarInfoBus.USUARIO_MODIFICACION = MvcApplication.UserName;
                                actualizarInfoBus.FECHA_MODIFICACION = DateTime.Now;
                                int rba = db.SaveChanges();

                                if (rba <= 0)
                                {
                                    Transaction.Current.Rollback();
                                    result.codigo = -2;
                                    result.mensaje = "No fue posible Actualizar Informacion el Bus Anterior";
                                    return result;
                                }

                                var busAsignado = (from ba in db.EVE01_INSCRIPCION_BUS
                                                   where ba.EVENTO == tienebusAsignado.EVENTO
                                                   && ba.PARTICIPANTE == tienebusAsignado.PARTICIPANTE
                                                   && ba.BUS == tienebusAsignado.BUS
                                                   select ba).SingleOrDefault();
                                db.EVE01_INSCRIPCION_BUS.Remove(busAsignado);
                                int re = db.SaveChanges();

                                if (re <= 0 )
                                {
                                    Transaction.Current.Rollback();
                                    result.codigo = -2;
                                    result.mensaje = "No fue posible eliminar el bus de la Inscripcion";
                                    return result;
                                }                                
                            }

                        }
                    }
                    tr.Complete();
                }
                result.codigo = 0;
                result.mensaje = "Se eliminaron las opciones agregadas en la Inscripcion";
                result.data = new InscripcionOpcion();
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al eliminar las opciones de la Inscripcion, Ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<List<InscripcionOpcion>> opcionesDeInscripcion(Participante data)
        {
            Respuesta<List<InscripcionOpcion>> result = new Respuesta<List<InscripcionOpcion>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de datos al obtener las opciones de la Inscripcion";
            result.data = new List<InscripcionOpcion>();            

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var opciones = (from o in db.EVE01_INSCRIPCION_OPCION
                                    orderby o.OPCION ascending
                                    where o.PARTICIPANTE == data.idParticipante
                                    && o.EVENTO == MvcApplication.idEvento
                                    select o).ToList();

                    if (opciones.Count == 0)
                    {
                        result.codigo = -1;
                        result.mensaje = "No existen Opciones Registrados en la Inscripcion";
                        return result;
                    }
                    else
                    {
                        foreach (var item in opciones)
                        {
                            result.data.Add(new InscripcionOpcion(item)
                            {
                                es_obligatorio = obligatorio(item.OPCION),
                                es_transporte = transporte(item.OPCION),
                                es_hospedaje = hospedaje(item.OPCION),                                
                                descripcion = nombre(item.OPCION),
                                estadoopcion = item.ESTADO_REGISTRO == "A" ? true: false,
                                precio = precioopcion(item.OPCION)
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
                
                throw;
            }
        }

        public Respuesta<InscripcionOpcion> modificarEstadoOpcion()
        {
            Respuesta<InscripcionOpcion> result = new Respuesta<InscripcionOpcion>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de Datos";
            result.data = new InscripcionOpcion();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var opcion = (from o in db.EVE01_INSCRIPCION_OPCION
                                      where o.EVENTO == MvcApplication.idEvento
                                      && o.PARTICIPANTE == this.idParticipante
                                      && o.OPCION == this.idOpcion
                                      select o).SingleOrDefault();


                        if (!this.estadoopcion && transporte(opcion.OPCION))//SE REALIZA LA VALIDACION DE TRANSPORTE
                        {
                            if (validaFechaAnulacionTransporte())//SE REALIZA LA VALIDACION DE LA FECHA LIMITE PARA ANULAR TRASPORTE
                            {
                                result.codigo = -1;
                                result.mensaje = "No es permitido cambiar el trasporte";
                                return result;
                            }
                            else
                            {
                                //SI EN LAS OPCIONES TIENE ASIGNADO BUS SE PROCEDE A ELIMINAR Y ACTUALIZAR LA DISPONIBILIDAD DE BUS PARA OTRO PARTICIPANTE
                                var tienebusAsignado = (from ob in db.EVE01_INSCRIPCION_BUS
                                                        where ob.EVENTO == MvcApplication.idEvento
                                                        && ob.PARTICIPANTE == this.idParticipante
                                                        select ob).SingleOrDefault();

                                if (tienebusAsignado != null)
                                {
                                    //SE PROCEDE A ACTUALIZAR LA DISPONIBILIDAD DE LOS BUSES
                                    var actualizarInfoBus = (from aib in db.EVE01_EVENTO_BUS
                                                             where aib.EVENTO == tienebusAsignado.EVENTO
                                                             && aib.BUS == tienebusAsignado.BUS
                                                             select aib).SingleOrDefault();

                                    actualizarInfoBus.DISPONIBLE++;
                                    actualizarInfoBus.OCUPADO--;
                                    actualizarInfoBus.USUARIO_MODIFICACION = MvcApplication.UserName;
                                    actualizarInfoBus.FECHA_MODIFICACION = DateTime.Now;
                                    int rba = db.SaveChanges();

                                    if (rba <= 0)
                                    {
                                        Transaction.Current.Rollback();
                                        result.codigo = -2;
                                        result.mensaje = "No fue posible Actualizar Informacion el Bus Anterior";
                                        return result;
                                    }

                                    var busAsignado = (from ba in db.EVE01_INSCRIPCION_BUS
                                                       where ba.EVENTO == tienebusAsignado.EVENTO
                                                       && ba.PARTICIPANTE == tienebusAsignado.PARTICIPANTE
                                                       && ba.BUS == tienebusAsignado.BUS
                                                       select ba).SingleOrDefault();
                                    db.EVE01_INSCRIPCION_BUS.Remove(busAsignado);
                                    int re = db.SaveChanges();

                                    if (re <= 0)
                                    {
                                        Transaction.Current.Rollback();
                                        result.codigo = -2;
                                        result.mensaje = "No fue posible eliminar el bus de la Inscripcion";
                                        return result;
                                    }
                                }
                                //SE CAMBIA EL ESTADO DE LA OPCION DE TRANSPORTE
                                opcion.ESTADO_REGISTRO = "B";
                            }
                        }
                        else if (this.estadoopcion)
                        {
                            opcion.ESTADO_REGISTRO = "A";
                        }
                        else
	                    {
                            opcion.ESTADO_REGISTRO = "B";
	                    }

                        opcion.USUARIO_MODIFICACION = MvcApplication.UserName;
                        opcion.FECHA_MODIFICACION = DateTime.Now;
                        int res_m = db.SaveChanges();

                        if (res_m <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "No fue posible actualizar el estado de la opcion";
                            return result;
                        }

                        //MODIFICACION DE SALDOS DEL PARTICIPANTE
                        SaldoParticipante modificar = new SaldoParticipante();
                        Respuesta<SaldoParticipante> respuesta = modificar.actualizarSaldos(this);

                        if (respuesta.codigo !=0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = respuesta.codigo;
                            result.mensaje = respuesta.mensaje;
                            return result;
                        }
                    }
                    tr.Complete();
                    result.codigo = 0;
                    result.mensaje = "Ok";
                    return result;                    
                }
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion al cambiar el estado dela opcion, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        #endregion

        #region Metodos Privados

        private bool obligatorio(decimal opcion)
        {
            using (var db = new EntitiesEVE01())
            {
                var res = (from op in db.EVE01_EVENTO_OPCION
                           where op.OPCION == opcion
                           && op.EVENTO == MvcApplication.idEvento
                           select op).SingleOrDefault();

                if (res.OBLIGATORIO == "S")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool transporte(decimal opcion)
        {
            using (var db = new EntitiesEVE01())
            {
                var res = (from op in db.EVE01_EVENTO_OPCION
                           where op.OPCION == opcion
                           && op.EVENTO == MvcApplication.idEvento
                           select op).SingleOrDefault();

                if (res.ES_TRANSPORTE == "S")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool validaFechaAnulacionTransporte()
        {
            using (var db = new EntitiesEVE01())
            {
                //PRIMERO SE OBTIENE LOS DATOS DEL BUS DEL INSCRITO
                var bus = (from b in db.EVE01_INSCRIPCION_BUS
                           where b.EVENTO == MvcApplication.idEvento
                           && b.PARTICIPANTE == this.idParticipante
                           select b).SingleOrDefault();

                if (bus == null)
                {
                    return false;
                }
                else
                {
                    //SEGUNDO SE OBTIENE LA INFORMACION DEL BUS DEL EVENTO
                    var fecha = (from f in db.EVE01_EVENTO_BUS
                                 where f.EVENTO == MvcApplication.idEvento
                                 && f.NO_BUS == bus.NO_BUS
                                 select f).SingleOrDefault();

                    DateTime hoy = DateTime.Now;
                    DateTime fechalimite = Convert.ToDateTime(fecha.FECHA_LIMITE_ANULACION);
                    int validarfecha = DateTime.Compare(hoy.Date, fechalimite.Date);

                    if (validarfecha > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private bool hospedaje(decimal opcion)
        {
            using (var db = new EntitiesEVE01())
            {
                var res = (from op in db.EVE01_EVENTO_OPCION
                           where op.OPCION == opcion
                           && op.EVENTO == MvcApplication.idEvento
                           select op).SingleOrDefault();

                if (res.ES_HOSPEDAJE == "S")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string nombre(decimal opcion)
        {
            using (var db = new EntitiesEVE01())
            {
                var res = (from op in db.EVE01_EVENTO_OPCION
                           where op.OPCION == opcion
                           && op.EVENTO == MvcApplication.idEvento
                           select op.DESCRIPCION).SingleOrDefault();

                return res;
            }
        }

        private Nullable<decimal> precioopcion(decimal opcion)
        {
            using (var db = new EntitiesEVE01())
            {
                var res = (from op in db.EVE01_EVENTO_OPCION
                           where op.OPCION == opcion
                           && op.EVENTO == MvcApplication.idEvento
                           select op.PRECIO).SingleOrDefault();

                return res;
            }
        }

        #endregion

    }
}