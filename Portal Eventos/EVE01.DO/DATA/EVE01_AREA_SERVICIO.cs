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
    
    public partial class EVE01_AREA_SERVICIO
    {
        public EVE01_AREA_SERVICIO()
        {
            this.EVE01_PARTICIPANTE_SERVICIO = new HashSet<EVE01_PARTICIPANTE_SERVICIO>();
        }
    
        public decimal AREA { get; set; }
        public string NOMBRE { get; set; }
        public string ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
    
        public virtual ICollection<EVE01_PARTICIPANTE_SERVICIO> EVE01_PARTICIPANTE_SERVICIO { get; set; }
    }
}
