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
    
    public partial class EVE01_MOVIMIENTO
    {
        public EVE01_MOVIMIENTO()
        {
            this.EVE01_RECIBO = new HashSet<EVE01_RECIBO>();
        }
    
        public decimal MOVIMIENTO { get; set; }
        public decimal PARTICIPANTE { get; set; }
        public Nullable<decimal> EVENTO { get; set; }
        public string TIPO_MOVIMIENTO { get; set; }
        public Nullable<decimal> TIPO_PAGO { get; set; }
        public Nullable<decimal> CANTIDAD { get; set; }
        public string ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
    
        public virtual EVE01_PARTICIPANTE EVE01_PARTICIPANTE { get; set; }
        public virtual EVE01_TIPO_MOVIMIENTO EVE01_TIPO_MOVIMIENTO { get; set; }
        public virtual EVE01_TIPO_PAGO EVE01_TIPO_PAGO { get; set; }
        public virtual EVE01_EVENTO EVE01_EVENTO { get; set; }
        public virtual ICollection<EVE01_RECIBO> EVE01_RECIBO { get; set; }
    }
}
