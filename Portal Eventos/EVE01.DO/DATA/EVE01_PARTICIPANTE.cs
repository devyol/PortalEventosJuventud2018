//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EVE01.DO.DATA
{
    using System;
    using System.Collections.Generic;
    
    public partial class EVE01_PARTICIPANTE
    {
        public EVE01_PARTICIPANTE()
        {
            this.EVE01_INSCRIPCION = new HashSet<EVE01_INSCRIPCION>();
            this.EVE01_PARTICIPANTE_SERVICIO = new HashSet<EVE01_PARTICIPANTE_SERVICIO>();
            this.EVE01_PARTICIPANTE_SALDO = new HashSet<EVE01_PARTICIPANTE_SALDO>();
            this.EVE01_INSCRIPCION_BUS = new HashSet<EVE01_INSCRIPCION_BUS>();
            this.EVE01_MOVIMIENTO = new HashSet<EVE01_MOVIMIENTO>();
            this.EVE01_INSCRIPCION_SILLA = new HashSet<EVE01_INSCRIPCION_SILLA>();
            this.EVE01_RECIBO = new HashSet<EVE01_RECIBO>();
            this.EVE01_INSCRIPCION_HOSPEDAJE = new HashSet<EVE01_INSCRIPCION_HOSPEDAJE>();
            this.EVE01_SILLA_ANULADA = new HashSet<EVE01_SILLA_ANULADA>();
        }
    
        public decimal PARTICIPANTE { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string CORREO { get; set; }
        public string TALLA { get; set; }
        public string GENERO { get; set; }
        public Nullable<System.DateTime> FECHA_NACIMIENTO { get; set; }
        public string ALERJICO { get; set; }
        public string OBSERVACIONES { get; set; }
        public string ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
    
        public virtual ICollection<EVE01_INSCRIPCION> EVE01_INSCRIPCION { get; set; }
        public virtual ICollection<EVE01_PARTICIPANTE_SERVICIO> EVE01_PARTICIPANTE_SERVICIO { get; set; }
        public virtual ICollection<EVE01_PARTICIPANTE_SALDO> EVE01_PARTICIPANTE_SALDO { get; set; }
        public virtual ICollection<EVE01_INSCRIPCION_BUS> EVE01_INSCRIPCION_BUS { get; set; }
        public virtual ICollection<EVE01_MOVIMIENTO> EVE01_MOVIMIENTO { get; set; }
        public virtual ICollection<EVE01_INSCRIPCION_SILLA> EVE01_INSCRIPCION_SILLA { get; set; }
        public virtual ICollection<EVE01_RECIBO> EVE01_RECIBO { get; set; }
        public virtual ICollection<EVE01_INSCRIPCION_HOSPEDAJE> EVE01_INSCRIPCION_HOSPEDAJE { get; set; }
        public virtual ICollection<EVE01_SILLA_ANULADA> EVE01_SILLA_ANULADA { get; set; }
    }
}
