using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class PhieuDieuChinhNhapKhoModel
    {
        public int ID_PhieuDieuChinhNhapKho { get; set; }
        public string SoPhieu { get; set; }
        public int ID_QLLH { get; set; }
        public int ID_PhieuNhap { get; set; }
        public int ID_NhanVien { get; set; }
        public string DienGiai { get; set; }
        public string UpdateBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public string TenPhieuNhap { get; set; }
        public string TenNhanVien { get; set; }

    }
}