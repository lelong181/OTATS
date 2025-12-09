using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class LichSuThanhToanModel
    {
        public int ID_DonHang { get; set; }
        public float SoTien { get; set; }
        public int LanGiao { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string GhiChu { get; set; }
        public string TenNhanVien { get; set; }
        public string ImageUrl { get; set; }
        public string HinhThucThanhToan { get; set; }
    }
}
