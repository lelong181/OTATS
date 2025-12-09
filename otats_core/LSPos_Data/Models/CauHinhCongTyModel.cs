using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class CauHinhCongTyModel
    {
        public string tenCongTy { get; set; }
        public string diaChi { get; set; }
        public string dienThoai { get; set; }
        public string email { get; set; }
        public int soPhutVaoDiemToiThieu { get; set; }
        public int thoiGianThongBaoGiaMoi { get; set; }
        public int dinhDangSo { get; set; }
        public string iconPath { get; set; }
        public bool suDungBangGiaLoaiKhachHang { get; set; }
        public int SoChuSoThapPhan { get; set; }
    }
}