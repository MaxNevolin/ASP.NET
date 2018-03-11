using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Diplom_project.Models;

namespace Diplom_project.Controllers
{
    public class HomeController : Controller
    {
        private mydbEntities webstoreDB = new mydbEntities();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            using (webstoreDB)
            {
                return View(webstoreDB.customer.ToList());
            }
        }


    }
}
