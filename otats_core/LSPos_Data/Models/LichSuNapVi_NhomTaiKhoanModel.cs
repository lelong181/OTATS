using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class LichSuNapVi_NhomTaiKhoanModel
    {
        public int ID { get; set; }
        public DateTime NgayTao { get; set; }
        public int ID_NhomTaiKhoan { get; set; }
        public string TenNhomTaiKhoan { get; set; }
        public decimal SoTien { get; set; }
        public string ImgUrl { get; set; }
        public int TrangThai { get; set; }
        public string CongThanhToan { get; set; }
        public string DuLieuThanhToan { get; set; }
    }
}
