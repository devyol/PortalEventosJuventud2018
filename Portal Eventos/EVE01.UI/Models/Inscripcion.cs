using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;
using System.Transactions;


namespace EVE01.UI.Models
{
    public class Inscripcion
    {
        #region Atributos Privados

        private EVE01_INSCRIPCION dbModel;

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

        public DateTime? fechaInscripcion
        {
            get { return dbModel.FECHA_INSCRIPCION; }
            set { dbModel.FECHA_INSCRIPCION = value; }
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

        #endregion

        #region Constructores

        public Inscripcion()
        {
            dbModel = new EVE01_INSCRIPCION();
        }

        public Inscripcion(EVE01_INSCRIPCION datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<Inscripcion> Inscribir()
        {
            Respuesta<Inscripcion> result = new Respuesta<Inscripcion>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de Datos";
            result.data = new Inscripcion();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var existeInscripcion = db.EVE01_INSCRIPCION.
                                            Where(i => i.EVENTO == MvcApplication.idEvento &&
                                                  i.PARTICIPANTE == this.idParticipante).
                                            Select(i => i).SingleOrDefault();

                    //SE VALIDA SI EXISTE EL REGISTRO DE LA INSCRIPCION
                    if (existeInscripcion == null)
                    {
                        //SI NO RETORNA VALOR, NO EXISTE EL REGISTRO POR LO QUE SE PROCEDE A INSERTAR LA INSCRIPCION
                        Respuesta<Inscripcion> objInscribir = this.insertInscripcion();
                        result.codigo = objInscribir.codigo;
                        result.mensaje = objInscribir.mensaje;
                        result.mensajeError = objInscribir.mensajeError;
                        return result;
                    }
                    else
                    {
                        //SI RETORNA VALOR, EXISTE EL REGISTRO POR LO QUE SE PROCEDE A REACTIVAR LA INSCRIPCION
                        Respuesta<Inscripcion> objInscribir = this.reInscribir();
                        result.codigo = objInscribir.codigo;
                        result.mensaje = objInscribir.mensaje;
                        result.mensajeError = objInscribir.mensajeError;
                        return result;
                    }
                }                
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion al momento de Inscribir, Ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }            
        }

        public Respuesta<Inscripcion> AnularInscripcion()
        {
            Respuesta<Inscripcion> result = new Respuesta<Inscripcion>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en base de datos";
            result.data = new Inscripcion();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var anular = db.EVE01_INSCRIPCION.
                                     Where(a => a.PARTICIPANTE == this.idParticipante &&
                                                a.EVENTO == MvcApplication.idEvento).
                                     Select(a => a).SingleOrDefault();

                        //SE CAMBIA EL ESTADO_REGISTRO A "B" PARA INDICAR QUE FUE ANULADA LA INSCRIPCION
                        anular.ESTADO_REGISTRO = "B";
                        anular.USUARIO_MODIFICACION = MvcApplication.UserName;
                        anular.FECHA_MODIFICACION = DateTime.Now;
                        int resp = db.SaveChanges();

                        if (resp <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "No fue Posible anular la inscripcion";
                            result.data = new Inscripcion();
                            return result;
                        }

                        //SE ELIMINAN LOS REGISTROS DE LAS OPCIONES DE LA INSCRIPCION
                        InscripcionOpcion objAnular = new InscripcionOpcion();
                        Respuesta<InscripcionOpcion> delOpciones = objAnular.eliminarOpcionesInscripcion(this);

                        if (delOpciones.codigo != 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = delOpciones.codigo;
                            result.mensaje = delOpciones.mensaje;
                            return result;
                        }

                        //SE ELIMINA EL REGISTRO DE SALDO DEL PARTICIPANTE
                        SaldoParticipante AnularSaldo = new SaldoParticipante();
                        Respuesta<SaldoParticipante> respuesta = AnularSaldo.anulacionSaldos(this);

                        if (respuesta.codigo != 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = respuesta.codigo;
                            result.mensaje = respuesta.mensaje;
                            return result;
                        }

                        //SE ANULAN LOS MOVIMIENTOS REALIZADOS
                        Movimiento anularMov = new Movimiento();
                        Respuesta<Movimiento> respuestaMov = anularMov.anularMovimientos(this.idParticipante);

                        if (respuestaMov.codigo != 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = respuestaMov.codigo;
                            result.mensaje = respuestaMov.mensaje;
                            return result;
                        }

                        //SE ANULA SILLA ASIGNADA
                        SillaAnulada anularSilla = new SillaAnulada();
                        anularSilla.idParticipante = this.idParticipante;
                        Respuesta<SillaAnulada> respuestaSilla = anularSilla.desasignarSillaAnulada();

                        if (respuestaSilla.codigo != 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = respuestaSilla.codigo;
                            result.mensaje = respuestaSilla.mensaje;
                            return result;                            
                        }

                    }
                    tr.Complete();
                    result.codigo = 0;
                    result.mensaje = "Se Anulo correctamente la Inscripcion";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion al Momento de Anular la Inscripcion, Ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }            
        }

        #endregion

        #region Metodos Privados

        private Respuesta<Inscripcion> insertInscripcion()
        {
            Respuesta<Inscripcion> res = new Respuesta<Inscripcion>();
            res.codigo = 1;
            res.mensaje = "Ocurrio un Error en base de datos";
            res.data = new Inscripcion();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        //SE INSERTA EN LA TABLA DE EVE01_INSCRIPCION PARA QUE SE CUMPLA LA INSCRIPCION
                        EVE01_INSCRIPCION ob = new EVE01_INSCRIPCION();
                        ob.EVENTO = MvcApplication.idEvento;
                        ob.PARTICIPANTE = this.idParticipante;
                        ob.FECHA_INSCRIPCION = DateTime.Now;
                        ob.ESTADO_REGISTRO = "A";
                        ob.USUARIO_CREACION = MvcApplication.UserName;
                        ob.FECHA_CREACION = DateTime.Now;
                        db.EVE01_INSCRIPCION.Add(ob);
                        int re_ob = db.SaveChanges();

                        if (re_ob <= 0)
                        {
                            Transaction.Current.Rollback();
                            res.codigo = 2;
                            res.mensaje = "No fue Posible Registrar la Inscripcion";
                            return res;
                        }

                        //SE AGREGAN LAS OPCIONES A LA INSCRIPCION
                        InscripcionOpcion agregar = new InscripcionOpcion();
                        Respuesta<InscripcionOpcion> objOpciones = agregar.agregarOpcionesInscripcion(this);

                        if (objOpciones.codigo != 0)
                        {
                            Transaction.Current.Rollback();
                            res.codigo = objOpciones.codigo;
                            res.mensaje = objOpciones.mensaje;
                            return res;
                        }
                    }
                    tr.Complete();
                    res.codigo = 0;
                    res.mensaje = "Se realizo la Inscripcion de Forma Correcta";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.codigo = -1;
                res.mensaje = "Ocurrio una excepcion al momento de la inscripcion, Ref: " + ex.ToString();
                res.mensajeError = ex.ToString();
                return res;
            }

        }

        private Respuesta<Inscripcion> reInscribir()
        {
            Respuesta<Inscripcion> res = new Respuesta<Inscripcion>();
            res.codigo = 1;
            res.mensaje = "Ocurrio un Error en base de datos";
            res.data = new Inscripcion();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        //SE ACTUALIZA EL CAMPO DEL ESTADO_REGISTRO AL VALOR "A"
                        var ob = db.EVE01_INSCRIPCION.
                                 Where(i => i.EVENTO == MvcApplication.idEvento && 
                                       i.PARTICIPANTE == this.idParticipante).
                                 Select(i => i).SingleOrDefault();

                        ob.ESTADO_REGISTRO = "A";
                        ob.USUARIO_CREACION = MvcApplication.UserName;
                        ob.FECHA_CREACION = DateTime.Now;
                        
                        int re_ob = db.SaveChanges();

                        if (re_ob <= 0)
                        {
                            Transaction.Current.Rollback();
                            res.codigo = 2;
                            res.mensaje = "No fue Posible Registrar la Inscripcion";
                            return res;
                        }

                        //SE AGREGAN LAS OPCIONES A LA INSCRIPCION
                        InscripcionOpcion agregar = new InscripcionOpcion();
                        Respuesta<InscripcionOpcion> objOpciones = agregar.agregarOpcionesInscripcion(this);

                        if (objOpciones.codigo != 0)
                        {
                            Transaction.Current.Rollback();
                            res.codigo = objOpciones.codigo;
                            res.mensaje = objOpciones.mensaje;                            
                            return res;
                        }
                    }
                    tr.Complete();
                    res.codigo = 0;
                    res.mensaje = "Se realizo la Inscripcion de Forma Correcta";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.codigo = -1;
                res.mensaje = "Ocurrio una excepcion al momento de la inscripcion, Ref: " + ex.ToString();
                res.mensajeError = ex.ToString();
                return res;
            }
        }

        #endregion

    }
}