using System;
using System.Linq;
using System.Web.Mvc;
using Website_Laptop.Models;

namespace Website_Laptop.Controllers
{
    public class SapXepController : Controller
    {
        DBLaptopDataContext db = new DBLaptopDataContext();

        public ActionResult SapXep(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.PriceDescSortParm = "price_desc";

            var products = from s in db.SANPHAMs
                           select s;

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.TENSP);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.GIA);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.GIA);
                    break;
                default:
                    products = products.OrderBy(p => p.TENSP);
                    break;
            }

            return View(products.ToList());
        }
    }
}
