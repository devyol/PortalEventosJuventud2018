using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.UI.Clases;
using EVE01.DO.DATA;


namespace EVE01.UI.Models
{
    public class InscripcionSilla
    {

        #region Atributos Privados

        private EVE01_INSCRIPCION_SILLA dbModel;        

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

        public InscripcionSilla()
        {
            dbModel = new EVE01_INSCRIPCION_SILLA();
        }

        public InscripcionSilla(EVE01_INSCRIPCION_SILLA datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<InscripcionSilla> infosillaAsignada()
        {
            Respuesta<InscripcionSilla> result = new Respuesta<InscripcionSilla>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en base de datos";
            result.data = new InscripcionSilla();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var infosilla = (from sa in db.EVE01_INSCRIPCION_SILLA
                                     where sa.EVENTO == MvcApplication.idEvento
                                     && sa.PARTICIPANTE == this.idParticipante
                                     select sa).SingleOrDefault();

                    if (infosilla != null)
                    {
                        dbModel = infosilla;
                    }

                }
                result.codigo = 0;
                result.mensaje = "OK";
                result.data = this;
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al obtener la informacion de la silla, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        #endregion

        #region Metodos Privados
        #endregion

    }
}