using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class PhieuNhapChiTietModel
    {
        public int ID_PhieuNhap { get; set; }
        public int ID_ChiTietPhieuNhap { get; set; }
        public int ID_HangHoa { get; set; }
        public float SoLuong { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
    }
}