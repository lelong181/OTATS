using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class BaoCaoKQKDOTAHangNgay
    {
        public string TenDaiLy { get; set; }
        public DateTime Ngay { get; set; }
        public List<ChiTiet_BaoCaoKQKDOTAHangNgay> lstDonHang { get; set; }

    }

    public class ChiTiet_BaoCaoKQKDOTAHangNgay
    {
        public DateTime ThoiGian { get; set; }
        public string TenKhachHang { get; set; }
        public string DienThoai { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public string TenDonVi { get; set; }
        public int SoLuong { get; set; }
        public float TongTien { get; set; }
    }
}
