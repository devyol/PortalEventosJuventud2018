using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.UI.Clases;
using EVE01.DO.DATA;

namespace EVE01.UI.Models
{
    public class ReciboDetalle
    {

        #region Atributos Privados

        private EVE01_RECIBO_DETALLE dbModel;        

        #endregion

        #region Propiedades Publicas

        public decimal idRecibo
        {
            get { return dbModel.RECIBO; }
            set { dbModel.RECIBO = value; }
        }

        public decimal idOpcion
        {
            get { return dbModel.OPCION; }
            set { dbModel.OPCION = value; }
        }

        public string descripcion
        {
            get { return dbModel.DESCRIPCION; }
            set { dbModel.DESCRIPCION = value; }
        }

        public decimal? precio
        {
            get { return dbModel.PRECIO; }
            set { dbModel.PRECIO = value; }
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

        public decimal? idParticipante { get; set; }
        #endregion

        #region Constructores

        public ReciboDetalle()
        {
            dbModel = new EVE01_RECIBO_DETALLE();
        }

        public ReciboDetalle(EVE01_RECIBO_DETALLE datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Respuesta<ReciboDetalle> guardarDetalle()
        {
            Respuesta<ReciboDetalle> result = new Respuesta<ReciboDetalle>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en base de datos";
            result.data = new ReciboDetalle();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var opcionesInscripcion = (from oi in db.EVE01_INSCRIPCION_OPCION
                                               where oi.EVENTO == MvcApplication.idEvento
                                               && oi.PARTICIPANTE == this.idParticipante
                                               && oi.ESTADO_REGISTRO == "A"
                                               select oi).ToList();

                    if (opcionesInscripcion.Count == 0)
                    {
                        result.codigo = -1;
                        result.mensaje = "No se puede registrar el detalle del recibo ya que no tiene opciones asignadas";
                        return result;
                    }
                    else
                    {
                        foreach (var item in opcionesInscripcion)
                        {
                            EVE01_RECIBO_DETALLE nuevo = new EVE01_RECIBO_DETALLE();
                            nuevo.RECIBO = this.idRecibo;
                            nuevo.OPCION = item.OPCION;
                            nuevo.DESCRIPCION = descripcionOpcion(item.OPCION);
                            nuevo.PRECIO = precioOpcion(item.OPCION);
                            nuevo.ESTADO_REGISTRO = "A";
                            nuevo.USUARIO_CREACION = MvcApplication.UserName;
                            nuevo.FECHA_CREACION = DateTime.Now;
                            db.EVE01_RECIBO_DETALLE.Add(nuevo);
                            db.SaveChanges();
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
                result.mensaje = "Ocurrio una excepcion al regristrar el detalle del recibo, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        #endregion

        #region Metodos Privados

        private string descripcionOpcion(decimal? opcion)
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

        private decimal? precioOpcion(decimal? opcion)
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