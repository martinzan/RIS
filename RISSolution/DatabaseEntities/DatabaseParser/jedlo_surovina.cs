//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class jedlo_surovina
    {
        public int id_surovina { get; set; }
        public int id_jedla { get; set; }
        public double mnozstvo { get; set; }
    
        public virtual jedlo jedlo { get; set; }
        public virtual surovina surovina { get; set; }
    }
}
