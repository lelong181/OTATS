using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Models
{
    public class CreateModelKeHoach
    {
        public int ID { get; set; }
        public int ID_NhanVien { get; set; }
        public int ID_KhachHang { get; set; }
        public int ID_DonHang { get; set; }
        public DateTime Ngay { get; set; }
        public string GhiChu { get; set; }
        public int ID_Xe { get; set; }
        public DateTime BatDau { get; set; }
        public DateTime KetThuc { get; set; }
        public string ViecCanLam { get; set; }
    }
}
