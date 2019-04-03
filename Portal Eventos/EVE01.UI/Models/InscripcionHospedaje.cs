using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;

namespace EVE01.UI.Models
{
    public class InscripcionHospedaje
    {
        #region Atributos Privados

        private EVE01_INSCRIPCION_HOSPEDAJE dbModel;

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

        public string encargado
        {
            get { return dbModel.ENCARGADO; }
            set { dbModel.ENCARGADO = value; }
        }

        public string telefono
        {
            get { return dbModel.TELEFONO; }
            set { dbModel.TELEFONO = value; }
        }

        public string direccion
        {
            get { return dbModel.DIRECCION; }
            set { dbModel.DIRECCION = value; }
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

        public InscripcionHospedaje()
        {
            dbModel = new EVE01_INSCRIPCION_HOSPEDAJE();
        }

        public InscripcionHospedaje(EVE01_INSCRIPCION_HOSPEDAJE datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<InscripcionHospedaje> datosHospedaje()
        {
            Respuesta<InscripcionHospedaje> result = new Respuesta<InscripcionHospedaje>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en base de datos";
            result.data = new InscripcionHospedaje();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var datos = (from d in db.EVE01_INSCRIPCION_HOSPEDAJE
                                 where d.EVENTO == MvcApplication.idEvento
                                 && d.PARTICIPANTE == this.idParticipante
                                 select d).SingleOrDefault();

                    if (datos != null)
                    {
                        dbModel = datos;
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
                result.mensaje = "Ocurrio una excepcion al obtener informacion del Hospedaje";
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<InscripcionHospedaje> registrarHospedaje()
        {
            Respuesta<InscripcionHospedaje> result = new Respuesta<InscripcionHospedaje>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de datos";
            result.data = new InscripcionHospedaje();

            try
            {
                using (var db = new EntitiesEVE01())
                {

                    var valhos = (from vh in db.EVE01_INSCRIPCION_HOSPEDAJE
                                  where vh.EVENTO == MvcApplication.idEvento
                                  && vh.PARTICIPANTE == this.idParticipante
                                  select vh).SingleOrDefault();

                    if (valhos != null)
                    {
                        valhos.ENCARGADO = this.encargado;
                        valhos.TELEFONO = this.telefono;
                        valhos.DIRECCION = this.direccion;
                        valhos.USUARIO_MODIFICACION = MvcApplication.UserName;
                        valhos.FECHA_MODIFICACION = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        EVE01_INSCRIPCION_HOSPEDAJE nuevo = new EVE01_INSCRIPCION_HOSPEDAJE();
                        nuevo.EVENTO = MvcApplication.idEvento;
                        nuevo.PARTICIPANTE = this.idParticipante;
                        nuevo.ENCARGADO = this.encargado;
                        nuevo.TELEFONO = this.telefono;
                        nuevo.DIRECCION = this.direccion;
                        nuevo.ESTADO_REGISTRO = "A";
                        nuevo.USUARIO_CREACION = MvcApplication.UserName;
                        nuevo.FECHA_CREACION = DateTime.Now;

                        db.EVE01_INSCRIPCION_HOSPEDAJE.Add(nuevo);
                        db.SaveChanges();
                    }
                }
                result.codigo = 0;
                result.mensaje = "Se ha registrado Correctamente el dato de Hospedaje del Participante";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al tratar de registrar Anotacion de Hospedaje, ref:  " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        #endregion

        #region Metodos Privados
        #endregion
    }
}