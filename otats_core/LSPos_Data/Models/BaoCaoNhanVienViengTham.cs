using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class BaoCaoNhanVienViengThamModel
    {
        public DateTime Ngay { get; set; }
        public string TenNhanVien { get; set; }
        public string TenTuyen { get; set; }
        public int TongSo { get; set; }
        public int DaViengTham { get; set; }
        public int ChuaViengTham { get; set; }
        public int SoDonHang { get; set; }
        public double TongTien { get; set; }
    }

    public class BaoCaoSoLuongNhanVienViengThamModel
    {
        public DateTime Ngay { get; set; }
        public string TenNhanVien { get; set; }
        public string TenTuyen { get; set; }
        public string TrangThai { get; set; }
        public string TenKhachHang { get; set; }
        public string DienThoai { get; set; }
    }
}