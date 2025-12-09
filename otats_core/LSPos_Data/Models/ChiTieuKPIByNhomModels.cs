using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ChiTieuKPIByNhomModels
    {
        public ChiTieuKPIByNhomModels()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DateTime ApDung_TuNgay { get; set; }
        public double DoanhSo { get; set; }
        public int IDQLLH { get; set; }
        public int ID_ChiTieuKPI { get; set; }
        public int ID_NhanVien { get; set; }
        public int ID_Nhom { get; set; }
        public double LuotViengTham { get; set; }
        public double NgayCong { get; set; }
        public double SoDonHang { get; set; }
        public int TrangThai { get; set; }
    }
}