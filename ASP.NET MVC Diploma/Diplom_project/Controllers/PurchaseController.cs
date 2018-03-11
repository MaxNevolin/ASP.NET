using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Diplom_project.Models;
using Diplom_project.ViewModels;
namespace Diplom_project.Controllers
{
    public class PurchaseController : Controller
    {
        //
        // GET: /Purchase/
        private mydbEntities webstoreDB = new mydbEntities();
       
          //
          // GET: /Checkout/AddressAndPayment

          public ActionResult ChooseAdress()
          {
              string s = System.Web.HttpContext.Current.User.Identity.Name;
              MembershipUser user = Membership.GetUser(s);  
              Customer webstoreUser=webstoreDB.customer.Single(c => c.email==user.UserName);
              List<Adress> listAdress = new List<Adress>();
              listAdress = webstoreDB.adress.Where(a => a.Customer_idCustomer == webstoreUser.idCustomer).ToList();
              var model = new AdressSelectViewModel
                {
                Items = listAdress.Select(u => new SelectListItem
                    {
                        Value = u.idAdress.ToString(),
                        Text = u.city+", "+u.apartment
                    })
                };
              model.SelectedItemId = 1;
              return View(model);
          }

        public int CreatePurchase(List <CartItem> carts, int idAdress , int idCustomer, double price)
        {
            try
            {
                Purchase purchase = new Purchase();
                purchase.Adress_idAdress=idAdress;
                purchase.Customer_idCustomer=idCustomer;
                purchase.datePurchase=System.DateTime.Today;
                if (webstoreDB.purchase.Any(o => o.idPurchase > 0))
                    purchase.idPurchase = webstoreDB.purchase.Max(p => p.idPurchase) + 1;
                else
                    purchase.idPurchase = 1;
                purchase.totalPrice=price;
                webstoreDB.purchase.Add(purchase);
                webstoreDB.SaveChanges();
                Prodinpurchase prodinpurch = new Prodinpurchase();
                prodinpurch.amount = 1;
                prodinpurch.Product_idProduct = 1;
                prodinpurch.Purchase_idPurchase = 1;
                prodinpurch.purchase = purchase;
                prodinpurch.product = webstoreDB.product.Single(p => p.idProduct == 1);
                webstoreDB.prodinpurchase.Add(prodinpurch);
                /*foreach(var c in carts)
                {                 
                    prodinpurch.amount = c.amount;
                    prodinpurch.Product_idProduct = c.idProduct;
                    prodinpurch.Purchase_idPurchase = purchase.idPurchase;
                    webstoreDB.prodinpurchase.Add(prodinpurch);
                }*/
                webstoreDB.SaveChanges();
                return  purchase.idPurchase;
            }
            catch
            {
                return 0;
            }
        }
          //
          // POST: /Checkout/AddressAndPayment

          [HttpPost]
        public ActionResult ChooseAdress(AdressSelectViewModel Adress)
          {
             
              var cart = ShoppingCart.GetCart(this.HttpContext);
              List <CartItem> listItems = cart.GetCartItems();
              
              string s = System.Web.HttpContext.Current.User.Identity.Name;
              int idCustomer= webstoreDB.customer.Single(c => c.email == s).idCustomer;
              
              List<Adress> listAdress = new List<Adress>();
              listAdress = webstoreDB.adress.Where(a => a.Customer_idCustomer == idCustomer).ToList();
              int idAdress = listAdress.ElementAt(Adress.SelectedItemId-1).idAdress;

              double custDiscount=(double)webstoreDB.customer.Single(c => c.email==s).customerdiscont.discountPercent;
              double price = (double)cart.GetTotal((decimal)custDiscount);
              int idPurchase;
              if((idPurchase=CreatePurchase(listItems,idAdress,idCustomer,price))!=0)
              {
                  return RedirectToAction("Complete",new { id = idPurchase});
              }
              else
                  return View();
          }

          //
          // GET: /Checkout/Complete

          public ActionResult Complete(int id)
          {
              // Validate customer owns this order
              string s = System.Web.HttpContext.Current.User.Identity.Name;
              int idCustomer= webstoreDB.customer.Single(c => c.email == s).idCustomer; 
              
              bool isValid = webstoreDB.purchase.Any(
                  p => p.idPurchase == id &&
                  p.Customer_idCustomer == idCustomer);

              if (isValid)
              {
                  return View(id);
              }
              else
              {
                  return View("Error");
              }
          }
          
    }
}
