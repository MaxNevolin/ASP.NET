//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diplom_project
{
    using System;
    using System.Collections.Generic;
    
    public partial class Warehousestate
    {
        public Warehousestate()
        {
            this.matinwarehouse = new HashSet<Matinwarehouse>();
        }
    
        public int idWarehouseState { get; set; }
        public System.DateTime dateState { get; set; }
    
        public virtual ICollection<Matinwarehouse> matinwarehouse { get; set; }
    }
}