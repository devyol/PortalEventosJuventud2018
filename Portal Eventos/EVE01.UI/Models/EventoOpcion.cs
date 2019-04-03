using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;
using EVE01.UI.Models;

namespace EVE01.UI.Models
{
    public class EventoOpcion
    {

        #region Atributos Privados

        private EVE01_EVENTO_OPCION dbModel;

        #endregion

        #region Propiedades Publicas

        public decimal idOpcion
        {
            get { return dbModel.OPCION; }
            set { dbModel.OPCION = value; }
        }

        public decimal? idEventoOpcion
        {
            get { return dbModel.EVENTO; }
            set { dbModel.EVENTO = value; }
        }

        public string Descripcion
        {
            get { return dbModel.DESCRIPCION; }
            set { dbModel.DESCRIPCION = value; }
        }

        public decimal? Precio
        {
            get { return dbModel.PRECIO; }
            set { dbModel.PRECIO = value; }
        }

        public string esObligatorio
        {
            get { return dbModel.OBLIGATORIO; }
            set { dbModel.OBLIGATORIO = value; }
        }

        public string esTransporte
        {
            get { return dbModel.ES_TRANSPORTE; }
            set { dbModel.ES_TRANSPORTE = value; }
        }

        public string esHospedaje
        {
            get { return dbModel.ES_HOSPEDAJE; }
            set { dbModel.ES_HOSPEDAJE = value; }
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

        public DateTime? fechaModficacion
        {
            get { return dbModel.FECHA_MODIFICACION; }
            set { dbModel.FECHA_MODIFICACION = value; }
        }

        public Estado valorEstado { get; set; }

        #endregion

        #region Constructores

        public EventoOpcion()
        {
            dbModel = new EVE01_EVENTO_OPCION();
        }

        public EventoOpcion(EVE01_EVENTO_OPCION datos)
        {
            dbModel = datos;
        }

        #endregion





    }
}