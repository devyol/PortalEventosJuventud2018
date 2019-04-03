using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;
using EVE01.UI.Clases;
using System.Transactions;

namespace EVE01.UI.Models
{
    public class Participante
    {

        #region Atributos Privados

        /// <summary>
        /// Esta Propiedad es la instancia del modelo de la base de datos para la tabla
        /// </summary>
        private EVE01_PARTICIPANTE dbModel;

        #endregion

        #region Propiedades Publicas

        
        /// <summary>
        /// Propiedad que expone la propiedad PARTICIPANTE del modelo de la base de datos
        /// </summary>
        public decimal idParticipante
        {
            get { return dbModel.PARTICIPANTE; }
            set { dbModel.PARTICIPANTE = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad NOMBRE del modelo de la base de datos
        /// </summary>
        public string nombre
        {
            get { return dbModel.NOMBRE; }
            set { dbModel.NOMBRE = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad APELLIDO del modelo de la base de datos
        /// </summary>
        public string apellido
        {
            get { return dbModel.APELLIDO; }
            set { dbModel.APELLIDO = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad DIRECCION del modelo de la base de datos
        /// </summary>
        public string direccion
        {
            get { return dbModel.DIRECCION; }
            set { dbModel.DIRECCION = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad TELEFONO del modelo de la base de datos
        /// </summary>
        public string telefono
        {
            get { return dbModel.TELEFONO; }
            set { dbModel.TELEFONO = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad CORREO del modelo de la base de datos
        /// </summary>
        public string correo
        {
            get { return dbModel.CORREO; }
            set { dbModel.CORREO = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad TALLA del modelo de la base de datos
        /// </summary>
        public string talla
        {
            get { return dbModel.TALLA; }
            set { dbModel.TALLA = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad GENERO del modelo de la base de datos
        /// </summary>
        public string genero
        {
            get { return dbModel.GENERO; }
            set { dbModel.GENERO = value; }
        }
        
        /// <summary>
        /// Propiedad que expone la propiedad FECHA_NACIMIENTO del modelo de la base de datos
        /// </summary>
        public DateTime? fechaNacimiento
        {
            get { return dbModel.FECHA_NACIMIENTO; }
            set { dbModel.FECHA_NACIMIENTO = value; }
        }

        
        /// <summary>
        /// Propiedad que expone la propiedad ALERJICO del modelo de la base de datos
        /// </summary>
        public string alerjico
        {
            get { return dbModel.ALERJICO; }
            set { dbModel.ALERJICO = value; }
        }

        /// <summary>
        /// Propiedad que expone la propiedad OBSERVACIONES del modelo de la base de datos
        /// </summary>
        public string observaciones
        {
            get { return dbModel.OBSERVACIONES; }
            set { dbModel.OBSERVACIONES = value; }
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
        /// Propiedad que expone la propiedad USUARIO_CREACION del modelo de la base de datos
        /// </summary>
        public string usuarioModificacion
        {
            get { return dbModel.USUARIO_MODIFICACION; }
            set { dbModel.USUARIO_MODIFICACION = value; }
        }

        /// <summary>
        /// Propiedad que expone la propiedad FECHA_CREACION del modelo de la base de datos
        /// </summary>
        public DateTime? fechaModificacion
        {
            get { return dbModel.FECHA_MODIFICACION; }
            set { dbModel.FECHA_MODIFICACION = value; }
        }

        public string NombreCompleto { get; set; }

        public bool validaActualizacionEvento { get; set; }

        public string validaInscripcion { get; set; }

        public bool validaInscripcionbool { get; set; }

        public Estado valorEstado { get; set; }

        #endregion

        #region Constructores

        /// <summary>
        /// Crea una instancia de la clase Participante y el atributo dbModel se inicializa con sus valores por defecto
        /// </summary>
        public Participante()
        {
            dbModel = new EVE01_PARTICIPANTE();
        }

        /// <summary>
        /// Crea una instancia de la clase Participante y el atriburo dbModel se inicializa con los valores del parametro datos
        /// </summary>
        /// <param name="datos">El modelo cuyos valores seran asignados al atributo dbModel</param>
        public Participante(EVE01_PARTICIPANTE datos)
        {
            dbModel = datos;
        }

        #endregion

        #region Metodos Publicos

        /// <summary>
        /// METODO QUE OBTIENE EL LISTADO DE PARTICIPANTES ACTIVOS
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Participante>> listaParticipatnesActivos()
        {
            Respuesta<List<Participante>> result = new Respuesta<List<Participante>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de Datos al obtener el listado de Participatnes";
            result.data = new List<Participante>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    var lista = db.EVE01_PARTICIPANTE.
                                Where(p => p.ESTADO_REGISTRO == "A").
                                Select(p => p).
                                OrderBy(p => p.APELLIDO).
                                ToList();
                    
                    if (lista.Count == 0)
                    {
                        result.codigo = -1;
                        result.mensaje = "No existen Participantes Registrados";
                        return result;
                    }
                    else
                    {
                        foreach (var item in lista)
                        {
                            result.data.Add(new Participante(item));
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
                result.mensaje = "Ocurrio una excepcion al obtener el listado de Participantes " + ex.ToString();
                return result;
            }

        }

        public Respuesta<Participante> obtenerInformacionParticipante()
        {
            Respuesta<Participante> result = new Respuesta<Participante>();
            result.codigo = 1;
            result.mensaje = "";
            result.data = new Participante();

            try
            {
                using (var db = new EntitiesEVE01())
                {

                    if (MvcApplication.idEvento == 0)
                    {
                        result.codigo = -1;
                        result.mensaje = "No se ha Iniciado un Evento para Inscripcion del Participante";
                        return result;
                    }

                    var infoParticipante = (from p in db.EVE01_PARTICIPANTE
                                            where p.PARTICIPANTE == this.idParticipante
                                            select p).SingleOrDefault();

                    if (infoParticipante == null)
                    {
                        result.codigo = -1;
                        result.mensaje = "No existe informacion para el Participante Indicado";
                        return result;
                    }
                    else
                    {
                        dbModel = infoParticipante;
                    }

                    this.validaActualizacionEvento = validaActualizacionDatosEvento();
                    this.NombreCompleto = apellidoNombreCompleto(this.idParticipante);
                    this.validaInscripcion = validarInscripcion(this.idParticipante);
                    this.validaInscripcionbool = validarInscripcionbool(this.idParticipante);
                    Estado objEstado = new Estado();
                    this.valorEstado = objEstado.valorEstado(infoParticipante.ESTADO_REGISTRO);

                }
                result.codigo = 0;
                result.mensaje = "Ok";
                result.data = this;
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al obtener la informacion del Participante " + ex.ToString();
                result.data = null;
                return result;
            }

        }

        public Respuesta<Participante> Nuevo()
        {
            Respuesta<Participante> result = new Respuesta<Participante>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de datos";
            result.data = new Participante();

            if (MvcApplication.idEvento == 0)
            {
                result.codigo = -1;
                result.data = new Participante();
                result.mensaje = "Para registrar una Participante Nuevo, debe iniciar un Evento";
                return result;
            }

            if (this.nombre == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del nombre no puede ser vacio, colocar un nombre valido";
                return result;
            }
            else if (this.apellido == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del apellido no puede ser vacio, colocar un apellido valido";
                return result;
            }
            else if (this.telefono == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del telefono no puede ser vacio, colocar un telefono valido";
                return result;
            }
            else if (this.genero == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del geneo no puede ser vacio, colocar un dato de geneo valido";
                return result;
            }
            else if (this.fechaNacimiento == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato de la Fecha de Nacimiento no puede ser vacio, colocar un dato de Fecha de Nacimiento valido";
                return result;                
            }

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var correlativo = (from c in db.EVE01_PARTICIPANTE
                                           select c.PARTICIPANTE).Max();

                        var corr = correlativo + 1;

                        correlativo = correlativo == null ? 1 : corr;

                        EVE01_PARTICIPANTE nuevo = new EVE01_PARTICIPANTE();
                        nuevo.PARTICIPANTE = correlativo;
                        nuevo.NOMBRE = this.nombre.ToUpper();
                        nuevo.APELLIDO = this.apellido.ToUpper();
                        nuevo.DIRECCION = this.direccion;
                        nuevo.TELEFONO = this.telefono;
                        nuevo.CORREO = this.correo;
                        nuevo.TALLA = this.talla;
                        nuevo.GENERO = this.genero;
                        nuevo.FECHA_NACIMIENTO = this.fechaNacimiento;
                        nuevo.ALERJICO = this.alerjico;
                        nuevo.OBSERVACIONES = this.observaciones;
                        nuevo.ESTADO_REGISTRO = "A";
                        nuevo.USUARIO_CREACION = MvcApplication.UserName;
                        nuevo.FECHA_CREACION = DateTime.Now;

                        db.EVE01_PARTICIPANTE.Add(nuevo);
                        int rgp = db.SaveChanges();

                        if (rgp <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "Ocurrio un error al registrar al participante";
                            return result;
                        }

                        EVE01_EVENTO_ACTUALIZACION actualizacion = new EVE01_EVENTO_ACTUALIZACION();
                        actualizacion.EVENTO = MvcApplication.idEvento;
                        actualizacion.PARTICIPANTE = correlativo;
                        actualizacion.ESTADO_REGISTO = "A";
                        actualizacion.USUARIO_CREACION = MvcApplication.UserName;
                        actualizacion.FECHA_CREACION = DateTime.Now;
                        db.EVE01_EVENTO_ACTUALIZACION.Add(actualizacion);
                        int ria = db.SaveChanges();

                        if (ria <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "Ocurrio un error al registrar la actualizacion de datos del evento actual";
                            return result;                            
                        }
                        result.data.idParticipante = correlativo;
                    }
                    tr.Complete();
                }
                result.codigo = 0;
                result.mensaje = "Se registro Correctamente al Participante: "+ this.nombre + " " + this.apellido;                
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion al tratar de Registrar al Participante, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }
        }

        public Respuesta<Participante> Actualizar()
        {
            Respuesta<Participante> result = new Respuesta<Participante>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un Error en Base de datos";
            result.data = new Participante();

            if (this.nombre == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del nombre no puede ser vacio, colocar un nombre valido";
                return result;
            }
            else if (this.apellido == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del apellido no puede ser vacio, colocar un apellido valido";
                return result;
            }
            else if (this.telefono == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del telefono no puede ser vacio, colocar un telefono valido";
                return result;
            }
            else if (this.genero == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato del geneo no puede ser vacio, colocar un dato de geneo valido";
                return result;
            }
            else if (this.fechaNacimiento == null)
            {
                result.codigo = -1;
                result.mensaje = "El dato de la Fecha de Nacimiento no puede ser vacio, colocar un dato de Fecha de Nacimiento valido";
                return result;
            }

            try
            {
                using (var tr = new TransactionScope())
                {
                    using (var db = new EntitiesEVE01())
                    {
                        var existente = (from e in db.EVE01_PARTICIPANTE
                                         where e.PARTICIPANTE == this.idParticipante
                                         select e).SingleOrDefault();

                        existente.NOMBRE = this.nombre.ToUpper();
                        existente.APELLIDO = this.apellido.ToUpper();
                        existente.DIRECCION = this.direccion;
                        existente.TELEFONO = this.telefono;
                        existente.CORREO = this.correo;
                        existente.TALLA = this.talla;
                        existente.GENERO = this.genero;
                        existente.FECHA_NACIMIENTO = this.fechaNacimiento;
                        existente.ALERJICO = this.alerjico;
                        existente.OBSERVACIONES = this.observaciones;
                        existente.USUARIO_MODIFICACION = MvcApplication.UserName;
                        existente.FECHA_MODIFICACION = DateTime.Now;

                        int rap = db.SaveChanges();

                        if (rap <= 0)
                        {
                            Transaction.Current.Rollback();
                            result.codigo = -2;
                            result.mensaje = "Ocurrio un error al tratar de actualizar los datos";
                            return result;
                        }

                        var validaActualizacionEvento = (from va in db.EVE01_EVENTO_ACTUALIZACION
                                                         where va.EVENTO == MvcApplication.idEvento
                                                         && va.PARTICIPANTE == this.idParticipante
                                                         select va).SingleOrDefault();

                        if (validaActualizacionEvento == null)
                        {
                            EVE01_EVENTO_ACTUALIZACION actualizacion = new EVE01_EVENTO_ACTUALIZACION();
                            actualizacion.EVENTO = MvcApplication.idEvento;
                            actualizacion.PARTICIPANTE = this.idParticipante;
                            actualizacion.ESTADO_REGISTO = "A";
                            actualizacion.USUARIO_CREACION = MvcApplication.UserName;
                            actualizacion.FECHA_CREACION = DateTime.Now;
                            db.EVE01_EVENTO_ACTUALIZACION.Add(actualizacion);
                            int ria = db.SaveChanges();

                            if (ria <= 0)
                            {
                                Transaction.Current.Rollback();
                                result.codigo = -2;
                                result.mensaje = "Ocurrio un error al registrar la actualizacion de datos del evento actual";
                                return result;
                            }                            
                        }
                    }
                    tr.Complete();
                }
                result.codigo = 0;
                result.mensaje = "Se actualizaron correctamente los datos del Participante";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una Excepcion al tratar de Registrar al Participante, ref: " + ex.ToString();
                result.mensajeError = ex.ToString();
                return result;
            }

        }

        #endregion

        #region Metodos Privados

        private bool validaActualizacionDatosEvento()
        {
            using (var db = new EntitiesEVE01())
            {
                var valida = db.EVE01_EVENTO_ACTUALIZACION.Where(v => v.PARTICIPANTE == this.idParticipante &&
                    v.EVENTO == MvcApplication.idEvento).Select(v => v).SingleOrDefault();

                if (valida == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }                
            }
        }

        private string apellidoNombreCompleto(decimal id)
        {
            using (var db = new EntitiesEVE01())
            {
                var nombre = db.EVE01_PARTICIPANTE.
                                Where(p => p.PARTICIPANTE == id).
                                Select(p => new Participante() { NombreCompleto = p.APELLIDO + ", " + p.NOMBRE }).
                                SingleOrDefault();

                return nombre.NombreCompleto;
            }
        }

        private string validarInscripcion(decimal id)
        {
            using (var db = new EntitiesEVE01())
            {
                var valida = db.EVE01_INSCRIPCION.
                                Where(v => v.EVENTO == MvcApplication.idEvento &&
                                      v.PARTICIPANTE == id).
                                Select(v => v).SingleOrDefault();

                if (valida == null)
                {                    
                    return "NO INSCRITO";
                }
                else if (valida.ESTADO_REGISTRO == "B")
	            {                 
                    return "INSCRIPCION ANULADA";
	            }
                else
                {                 
                    return "INSCRITO";
                }

            }
        }

        private bool validarInscripcionbool(decimal id)
        {
            using (var db = new EntitiesEVE01())
            {
                var valida = db.EVE01_INSCRIPCION.
                                Where(v => v.EVENTO == MvcApplication.idEvento &&
                                      v.PARTICIPANTE == id).
                                Select(v => v).SingleOrDefault();

                if (valida == null || valida.ESTADO_REGISTRO == "B")
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        #endregion

    }
}