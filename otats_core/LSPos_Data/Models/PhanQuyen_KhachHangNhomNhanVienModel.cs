using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class PhanQuyen_KhachHangNhomNhanVienModel
    {
        public int ID_Nhom { get; set; }
        public int ID_KhachHang { get; set; }
        public string ID_Quyen { get; set; }
        public string TenNhom { get; set; }
    }
}