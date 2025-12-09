using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class TuyenDTO
    {
        public int ID { get; set; }
        public int ID_NhanVien { get; set; }
        public int ID_QLLH { get; set; }
        public string MoTa { get; set; }
        public int SoLuongKhachHang { get; set; }
        public string TenTuyen { get; set; }
        public int SoLuongNhanVien { get; set; }
        public int SoLuongNhomNhanVien { get; set; }
    }
}