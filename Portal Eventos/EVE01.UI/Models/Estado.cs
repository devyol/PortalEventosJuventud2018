using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;


namespace EVE01.UI.Models
{
    public class Estado
    {

        #region Atributos Privados

        private EVE01_ESTADO dbModel;

        #endregion

        #region Propiedades Publicas

        public string idEstado
        {
            get { return dbModel.ESTADO; }
            set { dbModel.ESTADO = value; }
        }

        public string Descripcion
        {
            get { return dbModel.DESCRIPCION; }
            set { dbModel.DESCRIPCION = value; }
        }

        public string UsuarioCreacion
        {
            get { return dbModel.USUARIO_CREACION; }
            set { dbModel.USUARIO_CREACION = value; }
        }

        public DateTime? FechaCreacion
        {
            get { return dbModel.FECHA_CREACION; }
            set { dbModel.FECHA_CREACION = value; }
        }

        #endregion

        #region Constructores
        
        public Estado()
        {
            dbModel = new EVE01_ESTADO();
        }

        public Estado(EVE01_ESTADO datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        public Estado valorEstado(string estado)
        {
            Estado result;

            using (var db = new EntitiesEVE01())
            {
                var objEstado = (from e in db.EVE01_ESTADO
                                 where e.ESTADO == estado
                                 select e).SingleOrDefault();

                result = new Estado(objEstado);
            }
            return result;
        }

        #endregion





    }
}