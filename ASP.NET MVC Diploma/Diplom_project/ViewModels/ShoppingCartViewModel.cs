using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diplom_project.Models;

namespace Diplom_project.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public double CustDiscount { get; set; }
    }
}