using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class TrangThaiDonHangModel
    {
        public int ID_TrangThaiDonHang { get; set; }
        public int KetThuc { get; set; }
        public int MacDinh { get; set; }
        public string MauTrangThai { get; set; }
        public string TenTrangThai { get; set; }
    }
}