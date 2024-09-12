using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Website_Laptop.Models;
namespace Website_Laptop.Controllers
{
    public class SanPhamController : Controller
    {
        //
        // GET: /SanPham/
        DBLaptopDataContext db = new DBLaptopDataContext();
        public ActionResult MacPartial()
        {
            
            return View(db.SANPHAMs.Where(x=>x.MALOAI == "A").ToList());
        }
        public ActionResult WinPartial()
        {
            return View(db.LOAISPs.Where(x => x.MALOAI != "A").ToList());
        }

        // show all san pham
        public ActionResult ShowAll()
        {
            return View(db.SANPHAMs.ToList());
        }
        // san pham theo ten
        public ActionResult ShowName(string ma)
        {
            var lst = db.SANPHAMs.Where(x => x.MALOAI == ma).ToList();
            var name = db.LOAISPs.Single(x => x.MALOAI == ma);
            ViewBag.name = name.TENLOAI;
            return View(lst);
        }

        public ActionResult ShowNameMac(int ma)
        {
            var lst = db.SANPHAMs.Where(x => x.MASP == ma).ToList();

            var name = db.SANPHAMs.Single(x=>x.MASP == ma);
            ViewBag.name = name.TENSP;
            return View(lst);
        }


        // xem chi tiet
        public ActionResult Detail(int ma)
        {
            SANPHAM sp = db.SANPHAMs.Single(x => x.MASP == ma);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        // tim san pham
        public ActionResult Search(string txtSearch)
        {
            var lstsp = db.SANPHAMs.Where(s => s.TENSP.Contains(txtSearch)).ToList();

            if (lstsp.Count() == 0)
            {
                return HttpNotFound();
            }

            var ma = db.LOAISPs.FirstOrDefault(s => s.TENLOAI.Contains(txtSearch));
            ViewBag.name = ma.TENLOAI;
     
            return View(lstsp);
        }
	}
}