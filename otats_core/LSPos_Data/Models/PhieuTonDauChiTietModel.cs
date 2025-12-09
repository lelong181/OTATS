using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class PhieuTonDauChiTietModel
    {
        public int ID_PhieuTonDau { set; get; }
        public int ID_QLLH { set; get; }
        public int ID_ChiTietPhieuTonDau { set; get; }
        public int ID_HangHoa { set; get; }
        public float SoLuong { set; get; }
        public int ID_Kho { get; set; }
        public DateTime? NgayChot { get; set; }
        public string UpdateBy { get; set; }
        public string MaHang { set; get; }
        public string TenHang { set; get; }
        public string TenDonVi { set; get; }

    }
}