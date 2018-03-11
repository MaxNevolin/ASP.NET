using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diplom_project.Models;

namespace Diplom_project.Controllers
{
    public class WebStoreController : Controller
    {
        //
        // GET: /WebStore/

        private mydbEntities webstoreDB = new mydbEntities();
        public static List<string> productTypes = new List<string>() { "Браслети", "Брелоки", "Кольє", "Кільця жіночі", "Кільця обручальні", "Чоловічі вироби", "Сережки" };
        //
        // GET: /Store/

        public ActionResult Index()
        {

            return View(productTypes);
        }

        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(string type)
        {
            List<Product> tmplist = webstoreDB.product.ToList();
            List<Product> productList = tmplist.Where(p => p.typeProduct == type).ToList();
            // Retrieve Genre and its Associated Albums from database

            return View(productList);
        }

        public ActionResult DetailsGems(int idProd)
        {
            List<Gemsinprod> gemList = webstoreDB.gemsinprod.Where(p => p.Product_idProduct == idProd).ToList();
            foreach (var gemitem in gemList)
            {
                if(gemitem.gem==null)
                    gemitem.gem = webstoreDB.gem.Single(p => p.idGem == gemitem.Gem_idGem);
            }
            // Retrieve Genre and its Associated Albums from database

            return PartialView(gemList);
        }
        //
        // GET: /Store/Details/5

        public ActionResult Details(int id)
        {
            var product = webstoreDB.product.Find(id);
            return View(product);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public ActionResult ProductTypeMenu()
        {
            return PartialView(productTypes);
        }

    }
}
