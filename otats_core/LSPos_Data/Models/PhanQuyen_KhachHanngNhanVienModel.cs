using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class PhanQuyen_KhachHanngNhanVienModel
    {
        public int ID_NhanVien { get; set; }
        public int ID_KhachHang { get; set; }
        public string ID_Quyen { get; set; }
        public string TenDangNhap { get; set; }
        public string TenDayDu { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string TenNhom { get; set; }
        public DateTime NgaySinh { get; set; }
    }
}