using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class PhieuTonDauModel
    {
        public int ID_PhieuTonDau { set; get; }
        public int ID_QLLH { set; get; }
        public int ID_NhanVien { set; get; }
        public int ID_KhoHang { set; get; }
        public DateTime NgayChotTon { set; get; }
        public string DienGiai { set; get; }
        public string UpdateBy { get; set; }
    }
}