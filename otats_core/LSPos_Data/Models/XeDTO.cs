using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class XeDTO
    {
        public int ID { get; set; }
        public string BienKiemSoat { get; set; }
        public int NamSX { get; set; }
        public int SoCho { get; set; }
        public string TenNhanVien { get; set; }
        public int ID_NhanVien { get; set; }
        public DateTime NgayBDGanNhat { get; set; }
        public DateTime NgayBDTiepTheo { get; set; }
        public int ChuKy { get; set; }
        public string MoTa { get; set; }
        public string LoaiXe { get; set; }
    }
}