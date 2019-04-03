using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.UI.Clases;
using EVE01.DO.DATA;
using System.Transactions;

namespace EVE01.UI.Models
{
    public class SillaAnulada
    {
        #region Atributos Privados

        private EVE01_SILLA_ANULADA dbModel;        

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

        public decimal? noSilla
        {
            get { return dbModel.NO_SILLA; }
            set { dbModel.NO_SILLA = value; }
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

        #endregion

        #region Constructores

        public SillaAnulada()
        {
            dbModel = new EVE01_SILLA_ANULADA();
        }

        public SillaAnulada(EVE01_SILLA_ANULADA datos)
        {
            dbModel = datos;
        }


        #endregion

        #region Metodos Publicos

        public Respuesta<SillaAnulada> desasignarSillaAnulada()
        {
            Respuesta<SillaAnulada> result = new Respuesta<SillaAnulada>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en base de datos";
            result.data = new SillaAnulada();

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var actual = (from ac in db.EVE01_INSCRIPCION_SILLA
                                      where ac.EVENTO == MvcApplication.idEvento
                                      && ac.PARTICIPANTE == this.idParticipante
                                      select ac).SingleOrDefault();

                        if (actual != null)
                        {
                            //1. SE GUARDA REGISTRO DE LA SILLA EN BITACORA
                            EVE01_SILLA_ANULADA objAnulado = new EVE01_SILLA_ANULADA();
                            objAnulado.EVENTO = MvcApplication.idEvento;
                            objAnulado.PARTICIPANTE = actual.PARTICIPANTE;
                            objAnulado.NO_SILLA = actual.NO_SILLA;
                            objAnulado.ESTADO_REGISTRO = "A";
                            objAnulado.USUARIO_CREACION = MvcApplication.UserName;
                            objAnulado.FECHA_CREACION = DateTime.Now;
                            db.EVE01_SILLA_ANULADA.Add(objAnulado);
                            int rsa = db.SaveChanges();
                            if (rsa <= 0)
                            {
                                Transaction.Current.Rollback();
                                result.codigo = -2;
                                result.mensaje = "Ocurrio un Error al registrar la silla anulada";
                                return result;
                            }

                            //2. SE PROCEDE A ELIMINAR EL REGISTRO DE SILLA ASIGNADA
                            db.EVE01_INSCRIPCION_SILLA.Remove(actual);
                            int ves = db.SaveChanges();

                            if (ves <= 0)
                            {
                                result.codigo = -2;
                                result.mensaje = "Ocurrio un error al desasingar silla";
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
                result.mensaje = "Ocurrio una excepcion al intentar desasignar silla, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        #endregion

        #region Metodos Privados
        #endregion
    }
}