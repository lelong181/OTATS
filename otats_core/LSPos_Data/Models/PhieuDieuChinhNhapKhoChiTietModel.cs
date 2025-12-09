using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class PhieuDieuChinhNhapKhoChiTietModel
    {
        public int ID_PhieuDieuChinhNhapChiTiet { get; set; }
        public int ID_PhieuDieuChinhNhap { get; set; }
        public int ID_ChiTietPhieuNhap { get; set; }
        public int ID_HangHoa { get; set; }
        public float SoLuong { get; set; }
        public float SoLuongDieuChinh { get; set; }
        public int LoaiDieuChinh { get; set; }
        public string UpdateBy { get; set; }

        public float SoLuongPhieuNhap { get; set; }
        public string TenHang { get; set; }
        public string MaHang { get; set; }
        public string TenLoaiDieuChinh { get; set; }

    }
}