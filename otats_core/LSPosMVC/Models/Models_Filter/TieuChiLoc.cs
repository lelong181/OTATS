using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.DynamicLinq;

namespace LSPosMVC.Models.Models_Filter
{
    public class TieuChiLoc
    {
        public int ID_NhanVien { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int IdNhanVien { get; set; }
        public int IdMatHang { get; set; }
        public int idKhachHang { get; set; }
        public int trangthaixem { get; set; }
        public int ttht { get; set; }
        public int ttgh { get; set; }
        public int tttt { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public int donhangtaidiem { get; set; }
        public string ListIDNhom { get; set; }
        public Filter Filters { get; set; }

        public int IdTinh { get; set; }
        public int IdQuan { get; set; }
        public int IdLoaiKhachHang { get; set; }

        public int IdNhom { get; set; }
        public int ID_NV { get; set; }
    }
}
