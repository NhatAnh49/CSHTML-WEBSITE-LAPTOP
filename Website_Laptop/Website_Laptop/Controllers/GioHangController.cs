using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.IO;
using Website_Laptop.Models;
using System.Diagnostics;


namespace Website_Laptop.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/

        DBLaptopDataContext db = new DBLaptopDataContext();

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }

            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ma, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(x => x.sMASP == ma);

            if (sp == null)
            {

                sp = new GioHang(ma);
                lstGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSoLuong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl += lstGioHang.Sum(s => s.iSoLuong);

            }
            return tsl;
        }
        private double TongThanhTien()
        {
            double ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(s => s.dThanhTien);

            }
            return ttt;
        }

        public ActionResult HienThiGioHang()
        {

            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count() == 0)
            {
                ViewBag.TB = "Giỏ Hàng Rỗng!";

            }

            ViewBag.TSL = TongSoLuong();
            ViewBag.TTT = TongThanhTien();

            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TSL = TongSoLuong();

            return PartialView();

        }

        public ActionResult XoaSanPham(int ma)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Single(s => s.sMASP == ma);
            if (sp != null)
            {
                lstGioHang.RemoveAll(s => s.sMASP == ma);

            }

            return RedirectToAction("HienThiGioHang", "GioHang");
        }

        public ActionResult XoaTatCa()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("HienThiGioHang", "GioHang");
        }


        public ActionResult CapNhatGioHang(int ma, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sach = lstGioHang.Single(s => s.sMASP == ma);
            sach.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            return RedirectToAction("HienThiGioHang", "GioHang");

        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            List<GioHang> lstGioHang = LayGioHang();

            ViewBag.TSL = TongSoLuong();
            ViewBag.TTT = TongThanhTien();


            return View(lstGioHang);
        }


        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            HOADON ddh = new HOADON();
            TAIKHOAN kh = (TAIKHOAN)Session["taikhoan"];
            List<GioHang> lstGioHang = LayGioHang();

            ddh.MAKH = kh.MAKH;
            ddh.NGAYDAT = DateTime.Now; // ngay dat hàng
            ddh.TINHTRANG = 0; // chưa thanh toan
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NGAYGIAO = DateTime.Parse(ngaygiao);
            var tinhtrang = f["phuongthuc"];
            ddh.TINHTRANG = int.Parse(tinhtrang);
            if (ddh.TINHTRANG == 1)
            {
                ddh.DATHANHTOAN = "Đã Thanh Toán";
            }
            else
            {
                ddh.DATHANHTOAN = "Chưa Thanh Toán";
            }
            db.HOADONs.InsertOnSubmit(ddh);
            db.SubmitChanges();

            // Lưu thông tin hóa đơn vào Session để sử dụng sau này
            Session["HoaDon"] = ddh;

            // them chi tiet don hang
            foreach (var item in lstGioHang)
            {
                CHITIETHOADON ctDH = new CHITIETHOADON();
                ctDH.MAHD = ddh.MAHD;
                ctDH.MASP = item.sMASP;
                ctDH.SOLUONG = item.iSoLuong;
                ctDH.THANHTIEN = item.iGIA;
                db.CHITIETHOADONs.InsertOnSubmit(ctDH);
            }

            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("ThongBao");
        }


        public ActionResult ThongBao()
        {
            // Lấy thông tin hóa đơn từ Session
            HOADON ddh = Session["HoaDon"] as HOADON;
            // Lấy danh sách sản phẩm từ giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();

            // Kiểm tra xem dữ liệu đã được lấy thành công hay không
            Debug.WriteLine("Thông tin hóa đơn: " + (ddh != null ? ddh.MAHD.ToString() : "null"));
            Debug.WriteLine("Danh sách sản phẩm: " + (lstGioHang != null ? lstGioHang.Count.ToString() : "null"));

            // Truyền thông tin hóa đơn và danh sách sản phẩm vào ViewBag
            ViewBag.HoaDon = ddh;
            ViewBag.DanhSachSanPham = lstGioHang;
            Session["GioHang"] = null;
            return View();
        }



    }
}