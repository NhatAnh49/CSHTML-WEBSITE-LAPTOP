using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Website_Laptop.Models;
namespace Website_Laptop.Controllers
{
    public class NguoiDungController : Controller
    {
        //
        // GET: /NguoiDung/

        DBLaptopDataContext db = new DBLaptopDataContext();

        [HttpGet]
        public ActionResult DangKy()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(TAIKHOAN kh, FormCollection f)
        {

            var hoten = f["hoten"];
            var tendn = f["tendn"];
            var matkhau = f["matkhau"];
            var rematkhau = f["rematkhau"];
            var dienthoai = f["dienthoai"];
            var ngaysinh = String.Format("{0:MM/DD/YYYY}", f["ngaysinh"]);
            var email = f["email"];
            var diachi = f["diachi"];

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Tên đăng nhập không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được bỏ trống!";
            }

            if (String.IsNullOrEmpty(rematkhau))
            {

                ViewData["Loi4"] = "Nhập lại mật khẩu!";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi5"] = "Số điện thoại không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(ngaysinh))
            {
                ViewData["Loi6"] = "Ngày sinh không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi7"] = "Email không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi8"] = "Địa chỉ không được bỏ trống!";
            }

            if (!string.IsNullOrEmpty(hoten) && !string.IsNullOrEmpty(tendn) &&
                !string.IsNullOrEmpty(matkhau) && !string.IsNullOrEmpty(rematkhau) &&
                !string.IsNullOrEmpty(dienthoai) && !string.IsNullOrEmpty(ngaysinh) &&
                !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(diachi))
            {
                kh.HOTEN = hoten;
                kh.TENDN = tendn;
                kh.MATKHAU = matkhau;
                kh.NGAYSINH = DateTime.Parse(ngaysinh);
                kh.DIACHI = diachi;
                kh.EMAIL = email;
                kh.VAITRO = "USER";

                if (db.TAIKHOANs.Any(x => x.TENDN == kh.TENDN))
                {
                    ViewBag.TB = "Tài Khoản Đã Tồn Tại";
                }
                else
                {
                    db.TAIKHOANs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    return RedirectToAction("DangNhap", "NguoiDung");
                }

            }
            return View();
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var tendn = f["tendn"];
            var matkhau = f["matkhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống!";
            }
            if (!String.IsNullOrEmpty(tendn) && !String.IsNullOrEmpty(matkhau))
            {
                TAIKHOAN kh = db.TAIKHOANs.SingleOrDefault(x => x.TENDN == tendn && x.MATKHAU == matkhau);

                if (kh != null )
                {
                    
                    Session["taikhoan"] = kh;
                    return RedirectToAction("ShowAll", "SanPham");
                }
                else
                {
                    ViewBag.TBDN = "Tên đăng nhập hoặc mật khẩu sai!";

                }
            }
            return View();
        }

        public ActionResult ThongTinTaiKhoan()
        {
            
            TAIKHOAN tk = (TAIKHOAN)Session["taikhoan"];
          
            return View(tk);
        
        }

        public ActionResult DangXuat()
        {
            Session["taikhoan"] = null;
            return RedirectToAction("DangNhap");
        }
    
    }

}