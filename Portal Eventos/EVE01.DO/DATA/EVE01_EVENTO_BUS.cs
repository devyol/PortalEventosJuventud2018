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
    
    public partial class EVE01_EVENTO_BUS
    {
        public EVE01_EVENTO_BUS()
        {
            this.EVE01_INSCRIPCION_BUS = new HashSet<EVE01_INSCRIPCION_BUS>();
        }
    
        public decimal BUS { get; set; }
        public Nullable<decimal> EVENTO { get; set; }
        public Nullable<decimal> NO_BUS { get; set; }
        public Nullable<decimal> CAPACIDAD { get; set; }
        public Nullable<decimal> DISPONIBLE { get; set; }
        public Nullable<decimal> OCUPADO { get; set; }
        public string HORA_SALIDA { get; set; }
        public string ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_LIMITE_ANULACION { get; set; }
    
        public virtual ICollection<EVE01_INSCRIPCION_BUS> EVE01_INSCRIPCION_BUS { get; set; }
        public virtual EVE01_EVENTO EVE01_EVENTO { get; set; }
    }
}
