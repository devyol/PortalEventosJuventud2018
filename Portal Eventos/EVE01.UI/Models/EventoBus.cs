using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.UI.Clases;
using EVE01.DO.DATA;

namespace EVE01.UI.Models
{
    public class EventoBus
    {
        #region Atributos Privados

        private EVE01_EVENTO_BUS dbModel;        

        #endregion

        #region Propiedades Publicas

        public decimal idBus
        {
            get { return dbModel.BUS; }
            set { dbModel.BUS = value; }
        }

        public Nullable<decimal> idEvento
        {
            get { return dbModel.EVENTO; }
            set { dbModel.EVENTO = value; }
        }

        public Nullable<decimal> noBus
        {
            get { return dbModel.NO_BUS; }
            set { dbModel.NO_BUS = value; }
        }

        public Nullable<decimal> capacidad
        {
            get { return dbModel.CAPACIDAD; }
            set { dbModel.CAPACIDAD = value; }
        }

        public Nullable<decimal> disponible
        {
            get { return dbModel.DISPONIBLE; }
            set { dbModel.DISPONIBLE = value; }
        }

        public Nullable<decimal> ocupado
        {
            get { return dbModel.OCUPADO; }
            set { dbModel.OCUPADO = value; }
        }

        public string horaSalida
        {
            get { return dbModel.HORA_SALIDA; }
            set { dbModel.HORA_SALIDA = value; }
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

        public bool validabusasignado { get; set; }

        #endregion

        #region Constructores

        public EventoBus()
        {
            dbModel = new EVE01_EVENTO_BUS();
        }

        public EventoBus(EVE01_EVENTO_BUS datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos        

        public Respuesta<List<EventoBus>> informacionBuses()
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
                                 orderby b.BUS ascending
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
                            result.data.Add(new EventoBus(item));
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

        #endregion

        #region Metodos Privados        

        #endregion
    }
}