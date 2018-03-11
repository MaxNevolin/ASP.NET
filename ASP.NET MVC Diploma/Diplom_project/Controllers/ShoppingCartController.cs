using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diplom_project.Models;
using Diplom_project.ViewModels;

namespace Diplom_project.Controllers
{
    public class ShoppingCartController : Controller
    {
        private mydbEntities webstoreDB = new mydbEntities();
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            string s=System.Web.HttpContext.Current.User.Identity.Name;
            double custDiscount=(double)webstoreDB.customer.Single(c => c.email==s).customerdiscont.discountPercent;
            decimal totalprice = cart.GetTotal((decimal)custDiscount);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = totalprice,
                CustDiscount = custDiscount*100
            };
            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {

            // Retrieve the album from the database
            var addedProduct = webstoreDB.product
                .Single(p => p.idProduct == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string s = System.Web.HttpContext.Current.User.Identity.Name;
            double custDiscount = (double)webstoreDB.customer.Single(c => c.email == s).customerdiscont.discountPercent;
            // Get the name of the album to display confirmation
            string nameProduct = ShoppingCart.cartList.Single(item => item.idRecord == id).nameProduct;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = (nameProduct) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal((decimal)custDiscount),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}
