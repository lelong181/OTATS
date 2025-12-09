using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ViTriNhanVienGPSModel
    {
        public ViTriNhanVienGPSModel() { }
        public int idnhanvien { get; set; }
        public double KinhDo { get; set; }
        public double ViDo { get; set; }
        public string thoigiancapnhat { get; set; }
        public string tennhanvien { get; set; }
        public int dangtructuyen { get; set; }
        public int ID_Nhom { get; set; }
        public string anhdaidien { get; set; }
        public string thoigianguitoado { get; set; }
        public string TenDangNhap { get; set; }
        public string TinhTrangPin { get; set; }
    }
}