using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Diplom_project.Models;

namespace Diplom_project.Controllers
{
    public class AccountController : Controller
    {
        private mydbEntities webstoreDB = new mydbEntities();

        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        public bool CustomerInDB(WebstoreUser model)
        {
           List<Customer> customerList = webstoreDB.customer.ToList();
            foreach(var customer in customerList)
                if ((model.UserName == customer.email) && (model.Password == customer.password))
                {
                     return true;
                }
            return false;
        }

        [HttpPost]
        public ActionResult LogOn(WebstoreUser model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    if (CustomerInDB(model))
                    {
                        MigrateShoppingCart(model.UserName);
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult UserLogin()
        {
            string s=System.Web.HttpContext.Current.User.Identity.Name;
            MembershipUser user = Membership.GetUser(s);
            return PartialView(user);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        public ActionResult CreateAccount(RegisterModel model)
        {
            /*RegisterModel m = new RegisterModel();
            m.CustomerAdress = new Adress();
            m.WebstoreCustomer = new Customer();*/
            return View();
        }
        //
        // POST: /Account/Register

        public bool CreateUser(RegisterModel registerInfo)
        {

            Customerdiscont custdisc = new Customerdiscont();
            custdisc.idCustomerDiscont = webstoreDB.customerdiscont.Max(p => p.idCustomerDiscont)+1;
            custdisc.purchaseSum = 0;
            custdisc.discountPercent = 0;
            webstoreDB.customerdiscont.Add(custdisc);

            Customer customer=registerInfo.WebstoreCustomer;
            customer.idCustomer = webstoreDB.customer.Max(p => p.idCustomer) + 1;
            customer.CustomerDiscont_idCustomerDiscont = custdisc.idCustomerDiscont;
            webstoreDB.customer.Add(customer);

            Adress adress = registerInfo.CustomerAdress;
            adress.idAdress = webstoreDB.adress.Max(p => p.idAdress) + 1;
            adress.Customer_idCustomer = customer.idCustomer;
            webstoreDB.adress.Add(adress);
            webstoreDB.SaveChanges();

            return true;
        }

        [HttpPost]
        public ActionResult Register(RegisterModel regModel)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                if (CreateUser(regModel))
                {
                    Membership.CreateUser(regModel.WebstoreCustomer.email, regModel.WebstoreCustomer.password, "customer", null, null, true, null, out createStatus);
                    if (createStatus == MembershipCreateStatus.Success)
                    {

                        MigrateShoppingCart(regModel.WebstoreCustomer.email);
                        FormsAuthentication.SetAuthCookie(regModel.WebstoreCustomer.email, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(regModel);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

     /*   [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true );
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            return View(model);
        }*/

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}


