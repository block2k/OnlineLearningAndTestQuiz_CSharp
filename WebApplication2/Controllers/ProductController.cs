using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        ProductOrderEntities db = new ProductOrderEntities();
        public ActionResult Index()
        {
            var listProduct = from listP in db.tblProducts select listP;
            return View(listProduct);
        }

        public ActionResult Index1()
        {
            var listProduct = from listP in db.tblProducts select listP;
            return View(listProduct);
        }

        public ActionResult Test()
        {
            Class1 class1 = new Class1();
            ViewBag.djtme = class1;
            return View();
        }
    }
}