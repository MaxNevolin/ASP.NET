using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_project.Models
{
    public class CartItem
    {
        public int idRecord { get; set; }
        public string idCart { get; set; }
        public int idProduct { get; set; }
        public int amount { get; set; }
        public decimal price { get; set; }
        public string nameProduct { get; set; }

    }
}