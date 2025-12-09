using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class KhachHang_HangGuiModel
    {
        public int ID { get; set; }
        public int ID_QLLH { get; set; }
        public int ID_KhachHang { get; set; }
        public int ID_MatHang { get; set; }
        public string TenKhachHang { get; set; }
        public string TenMatHang { get; set; }
        public string DiaChi { get; set; }
        public string TenDonVi { get; set; }
        public string MaHang { get; set; }
        public string DienThoai { get; set; }
        public string NguoiLienHe { get; set; }

        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public string DienGiai { get; set; }
        
        public string UpdatedBy { get; set; }
    }
}