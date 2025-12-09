using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class NhomModels
    {
        public int ID_Nhom { get; set; }
        public int ID_PARENT { get; set; }
        public int ID_QLLH { get; set; }
        public string MaNhom { get; set; }
        public string TenNhom { get; set; }
        public bool isChecked { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuongNhanVien { get; set; }
        public int SoLuongQuanLy { get; set; }
        public string TenHienThi_NhanVien { get; set; }
        public string TenHienThi_QuanLy { get; set; }
        public int TrangThai { get; set; }

        public List<NhomModels> childs { get; set; }
    }
}