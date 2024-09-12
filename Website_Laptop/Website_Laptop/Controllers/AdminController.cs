using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Website_Laptop.Models;

namespace Website_Laptop.Controllers
{


    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        apiAdminController api = new apiAdminController();

        DBLaptopDataContext db = new DBLaptopDataContext();

        public ActionResult admin()
        {
            return View();

        }



        // THÊM SP QUA API
        public ActionResult themSP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult themSP(FormCollection f)
        {
            SANPHAM sp = new SANPHAM();



            sp.MALOAI = f["maloai"];
            sp.TENSP = f["tensp"];
            sp.NSX = f["nsx"];
            sp.ANH = f["hinhanh"];
            sp.MOTA = f["mota"];
            sp.TGBH = f["tgbh"];


            int giaValue;
            if (int.TryParse(f["gia"], out giaValue))
            {
                sp.GIA = giaValue;
            }
            else
            {
                sp.GIA = null;
            }
            int slValue;
            if (int.TryParse(f["sl"], out slValue))
            {
                sp.SOLUONG = slValue;
            }
            else
            {
                sp.SOLUONG = null;
            }



            if (String.IsNullOrEmpty(sp.MALOAI))
            {
                ViewData["Loi1"] = "Mã loại không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(sp.TENSP))
            {
                ViewData["Loi2"] = "Tên sản phẩm không được bỏ trống!";
            }
            if (sp.GIA == null)
            {
                ViewData["Loi3"] = "Giá bán không được bỏ trống!";
            }
            if (sp.SOLUONG == null)
            {
                ViewData["Loi4"] = "Số lượng tồn không được bỏ trống!";
            }



            if (!string.IsNullOrEmpty(sp.MALOAI) && !string.IsNullOrEmpty(sp.TENSP) && sp.GIA != null && sp.SOLUONG != null)
            {
                if (api.ThemSanPham(sp) == 0)
                {
                    ViewBag.TT = "Thêm Sản Phẩm Thất Bại!";
                }
                else
                {
                    ViewBag.TT = "Thêm Sản Phẩm Thành Công!";
                }


            }

            return View();

        }


        // xóa sp qua api
        public ActionResult XoaSP(int ma)
        {
            api.XoaSanPham(ma);
            return RedirectToAction("QuanLiSP");
        }

        //quản lý sản phẩm
        public ActionResult QuanLiSP()
        {
            ViewBag.SL = db.SANPHAMs.Count();

            return View(api.ShowAll());
        }



        // Update sản phẩm
        public ActionResult CapNhatSP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CapNhatSP(int ma, FormCollection f)
        {

            SANPHAM sp = new SANPHAM();
            sp.MALOAI = f["maloai"];
            sp.TENSP = f["tensp"];
            sp.NSX = f["nsx"];
            sp.ANH = f["hinhanh"];
            sp.MOTA = f["mota"];
            sp.TGBH = f["tgbh"];
            int giaValue;
            if (int.TryParse(f["gia"], out giaValue))
            {
                sp.GIA = giaValue;
            }
            else
            {
                sp.GIA = null;
            }
            int slValue;
            if (int.TryParse(f["sl"], out slValue))
            {
                sp.SOLUONG = slValue;
            }
            else
            {
                sp.SOLUONG = null;
            }


            if (api.CapNhatSanPham(sp, ma) == 1)
            {
                ViewBag.TT = "Cập Nhật Sản Phẩm Thành Công!";
            }
            else
                ViewBag.TT = "Cập Nhật Sản Phẩm Thất Bại!";
            
            return View();

        }





    }
}