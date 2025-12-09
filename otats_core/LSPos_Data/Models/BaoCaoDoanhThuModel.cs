using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class BaoCaoDoanhThuModel
    {
        public string TimeText { get; set; }
        public double TongTien { get; set; }
        public int SoDonHang { get; set; }
    }

    public class BaoCaoSanLuongModel
    {
        public string TimeText { get; set; }
        public double SoLuong { get; set; }
        public int SoDonHang { get; set; }
    }
}