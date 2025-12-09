using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class NhanVienWebModels
    {
        public int ID_QuanLy { get; set; }
        public int ID_QLLH { get; set; }
        public int ID_Cha { get; set; }
        public int Level { get; set; }
        public string TenDangNhap { get; set; }
        public string TenDayDu { get; set; }
        public string MatKhau { get; set; }
        public List<int> DanhSachNhom { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}