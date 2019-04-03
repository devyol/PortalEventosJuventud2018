using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;
using System.Collections;
using System.Configuration;

namespace EVE01.UI.Models
{
    public class Evento
    {

        #region Atributos Privados

        /// <summary>
        /// Esta Propiedad es la instancia del modelo de la base de datos para la tabla
        /// </summary>
        private EVE01_EVENTO dbModel;

        #endregion

        #region Propiedades Publicas

        /// <summary>
        /// Propiedad que expone la propiedad EVENTO del modelo de la base de datos
        /// </summary>
        public decimal idEvento
        {
            get { return dbModel.EVENTO; }
            set { dbModel.EVENTO = value; }
        }        

        /// <summary>
        /// Propiedad que expone la propiedad NOMBRE_EVENTO del modelo de la base de datos
        /// </summary>
        public string Nombre
        {
            get { return dbModel.NOMBRE_EVENTO; }
            set { dbModel.NOMBRE_EVENTO = value; }
        }
                
        /// <summary>
        /// Propiedad que expone la propiedad FECHA_INICIO del modelo de la base de datos
        /// </summary>
        public string fechaInicio
        {
            get { return dbModel.FECHA_INICIO; }
            set { dbModel.FECHA_FIN = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad FECHA_FIN del modelo de la base de datos
        /// </summary>
        public string fechaFin
        {
            get { return dbModel.FECHA_FIN; }
            set { dbModel.FECHA_FIN = value; }
        }

        /// <summary>
        /// Propiedad que expone la propiedad APLICA_BUS del modelo de la base de datos
        /// </summary>
        public string aplicaBus
        {
            get { return dbModel.APLICA_BUS; }
            set { dbModel.APLICA_BUS = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad ESTADO_REGISTRO del modelo de la base de datos
        /// </summary>
        public string estadoRegistro
        {
            get { return dbModel.ESTADO_REGISTRO; }
            set { dbModel.ESTADO_REGISTRO = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad USUARIO_CREACION del modelo de la base de datos
        /// </summary>
        public string usuarioCreacion
        {
            get { return dbModel.USUARIO_CREACION; }
            set { dbModel.USUARIO_CREACION = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad FECHA_CREACION del modelo de la base de datos
        /// </summary>
        public DateTime? fechaCreacion
        {
            get { return dbModel.FECHA_CREACION; }
            set { dbModel.FECHA_CREACION = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad USUARIO_MODIFICACION del modelo de la base de datos
        /// </summary>
        public string usuarioModificacion
        {
            get { return dbModel.USUARIO_MODIFICACION; }
            set { dbModel.USUARIO_MODIFICACION = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad FECHA_MODIFICACION del modelo de la base de datos
        /// </summary>
        public DateTime? fechaModificacion
        {
            get { return dbModel.FECHA_MODIFICACION; }
            set { dbModel.FECHA_MODIFICACION = value; }
        }

        public string nombreActivo { get; set; }
        public bool activo { get; set; }

        public Estado valorEstado { get; set; }
        
        
        #endregion

        #region Constructores

        /// <summary>
        /// Crea una instancia de la clase Evento y el atributo dbModel se inicializa con sus valores por defecto
        /// </summary>
        public Evento()
        {
            dbModel = new EVE01_EVENTO();
        }

        /// <summary>
        /// Crea una instancia de la clase Evento y el atriburo dbModel se inicializa con los valores del parametro datos
        /// </summary>
        /// <param name="datos">El modelo cuyos valores seran asignados al atributo dbModel</param>
        public Evento(EVE01_EVENTO datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        /// <summary>
        /// Metodo que retorna Listado de Eventos Activos
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Evento>> ListarEventosActivos()
        {
            Respuesta<List<Evento>> result = new Respuesta<List<Evento>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en base de datos al obtener el listado";
            result.data = new List<Evento>();

            try
            {
                using (var db = new EntitiesEVE01())
                {

                    var listadoEvento = db.EVE01_EVENTO
                                        .Where(e => e.ESTADO_REGISTRO == "A")
                                        .Select(e => e).OrderByDescending(e =>e.EVENTO).ToList();

                    if (listadoEvento.Count > 0)
                    {
                        foreach (var item in listadoEvento)
                        {
                            result.data.Add(new Evento(item));
                        }
                    }
                    else
                    {
                        result.codigo = -1;
                        result.data = new List<Evento>();
                        result.mensaje = "No existen registros de Eventos Activos";
                        return result;
                    }
                }
                result.codigo = 0;
                result.mensaje = "OK";
                return result;

            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion, Referencia: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<List<Evento>> ListadoEventos()
        {
            Respuesta<List<Evento>> result = new Respuesta<List<Evento>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de datos, validar con el Administrador del Sistema";
            result.data = new List<Evento>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    Estado ObjEstado = new Estado();

                    var listadoEventos = db.EVE01_EVENTO
                                         .Select(e => e).OrderByDescending(e => e.EVENTO).ToList();

                    if (listadoEventos.Count > 0)
                    {
                        foreach (var item in listadoEventos)
                        {
                            result.data.Add(new Evento(item) { valorEstado = ObjEstado.valorEstado(item.ESTADO_REGISTRO) });
                        }
                    }
                    else
                    {
                        result.codigo = -1;
                        result.data = new List<Evento>();
                        result.mensaje = "No existen registros de Eventos";
                        return result;
                    }
                }
                result.codigo = 0;
                result.mensaje = "OK";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion, Referencia: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }


        }

        public Respuesta<IEnumerable> nombreUsuario()
        {
            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var respuesta = db.EVE01_USUARIO
                                    .Where(u => u.USUARIO == MvcApplication.UserName)
                                    .Select(u => u.NOMBRE).SingleOrDefault();
                    return new Respuesta<IEnumerable>() { codigo = 0, mensaje = "Ok", data = respuesta };
                    
                }
            }
            catch (Exception ex)
            {
                return new Respuesta<IEnumerable>() { codigo = -1, mensaje = "No existe nombre para el usuario", mensajeError = ex.ToString() };

            }

        }

        public Respuesta<Evento> validaEventoActivo()
        {
            Respuesta<Evento> result = new Respuesta<Evento>();
            result.data = new Evento();

            if (MvcApplication.EventoActivo == null||MvcApplication.EventoActivo=="")
            {
                result.data.activo = false;
                result.data.nombreActivo = ConfigurationManager.AppSettings["Mensaje"].ToString();
            }
            else
            {
                result.data.activo = true;
                result.data.nombreActivo = MvcApplication.EventoActivo;
            }
            result.codigo = 0;
            result.mensaje = "";
            return result;
        }

        public Respuesta<Evento> SetEventoActivo()
        {
            Respuesta<Evento> result = new Respuesta<Evento>();
            result.data = new Evento();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var nombre = db.EVE01_EVENTO.Where(e => e.EVENTO == this.idEvento).Select(e => e.NOMBRE_EVENTO).SingleOrDefault();
                    MvcApplication.EventoActivo = nombre;
                }
                MvcApplication.idEvento = this.idEvento;

                result.codigo = 0;
                result.mensaje = ConfigurationManager.AppSettings["Mensaje_Ok_1"].ToString() + MvcApplication.EventoActivo + ConfigurationManager.AppSettings["Mensaje_Ok_2"].ToString();
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio un inconveniente al tratar de Iniciar el Evento: " + ex.ToString();
                return result;
            }
        }

        #endregion



        #region Metodos Privados





        #endregion



    }
}