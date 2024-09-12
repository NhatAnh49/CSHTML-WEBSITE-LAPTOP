using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website_Laptop.Models
{
    public class GioHang
    {
        DBLaptopDataContext db = new DBLaptopDataContext();


        public int sMASP { get; set; }
        public string sTENSP { get; set; }
        public string sANH { get; set; }
        public int iGIA { get; set; }
        public int iSoLuong { get; set; }


        public double dThanhTien
        {
            get { return iGIA * iSoLuong; }
        }


        // khoi tao gio hang

        public GioHang(int MASP)
        {
            sMASP = MASP;
            SANPHAM sp = db.SANPHAMs.Single(s => s.MASP == sMASP);
            sTENSP = sp.TENSP;
            sANH = sp.ANH;
            iGIA = int.Parse(sp.GIA.ToString());
            iSoLuong = 1;
        }
    }
}