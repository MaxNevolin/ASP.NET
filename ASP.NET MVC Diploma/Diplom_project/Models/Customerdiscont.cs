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
    
    public partial class Customerdiscont
    {
        public Customerdiscont()
        {
            this.customer = new HashSet<Customer>();
        }
    
        public int idCustomerDiscont { get; set; }
        public double purchaseSum { get; set; }
        public Nullable<double> discountPercent { get; set; }
    
        public virtual ICollection<Customer> customer { get; set; }
    }
}