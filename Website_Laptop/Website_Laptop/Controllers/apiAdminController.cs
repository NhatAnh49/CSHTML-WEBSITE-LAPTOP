using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Website_Laptop.Models;

namespace Website_Laptop.Controllers
{
    public class apiAdminController : ApiController
    {
        DBLaptopDataContext db = new DBLaptopDataContext();

        [HttpGet]
        public List<SANPHAM> ShowAll()
        {
            return db.SANPHAMs.ToList();
        }


        [HttpPost]
        public int ThemSanPham(SANPHAM sp)
        {
            try
            {


                SANPHAM newsp = new SANPHAM();
                newsp.MALOAI = sp.MALOAI;
                newsp.TENSP = sp.TENSP;
                newsp.NSX = sp.NSX;
                newsp.ANH = sp.ANH;
                newsp.MOTA = sp.MOTA;
                newsp.TGBH = sp.TGBH;
                newsp.GIA = sp.GIA;
                newsp.SOLUONG = sp.SOLUONG;
                db.SANPHAMs.InsertOnSubmit(newsp);
                db.SubmitChanges();
                return 1;

            }
            catch
            {
                return 0;
            }

        }

        [HttpPut]
        public int CapNhatSanPham(SANPHAM sp, int ma)
        {
            try
            {
                SANPHAM newsp = db.SANPHAMs.FirstOrDefault(x => x.MASP == ma);
                if (newsp == null)
                    return 0;


                if (sp.MALOAI != "")
                {
                    newsp.MALOAI = sp.MALOAI;
                }
                if (sp.TENSP != "")
                { 
                    newsp.TENSP = sp.TENSP;               
                }
                if (sp.NSX != "")
                {
                    newsp.NSX = sp.NSX;
                }
                if (sp.ANH != "")
                {
                    newsp.ANH = sp.ANH;
                }
                if (sp.MOTA != "")
                {
                    newsp.MOTA = sp.MOTA;
                }
                if (sp.TGBH != "")
                {
                    newsp.TGBH = sp.TGBH;

                }
                if (sp.GIA != null)
                {
                    newsp.GIA = sp.GIA;

                }
                if (sp.SOLUONG != null)
                {
                    newsp.SOLUONG = sp.SOLUONG;

                }
                db.SubmitChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        [HttpDelete]
        public int XoaSanPham(int ma)
        {
            SANPHAM sp = db.SANPHAMs.FirstOrDefault(x => x.MASP == ma);
            if (sp == null) return 0;
            db.SANPHAMs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return 1;

        }


    }
}
