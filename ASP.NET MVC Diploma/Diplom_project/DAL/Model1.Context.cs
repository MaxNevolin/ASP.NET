﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mydbEntities : DbContext
    {
        public mydbEntities()
            : base("name=mydbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Adress> adress { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Customerdiscont> customerdiscont { get; set; }
        public DbSet<Gem> gem { get; set; }
        public DbSet<Gemsinprod> gemsinprod { get; set; }
        public DbSet<Material> material { get; set; }
        public DbSet<Prodinpurchase> prodinpurchase { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<Productdiscont> productdiscont { get; set; }
        public DbSet<Purchase> purchase { get; set; }
        public DbSet<Matinwarehouse> matinwarehouse { get; set; }
        public DbSet<Warehousestate> warehousestate { get; set; }
    }
}
