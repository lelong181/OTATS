using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class CongViecModel
    {
        public int ID_CongViec { set; get; }
        public int ID_QLLH { set; get; }
        public int ID_NguoiGui { set; get; }
        public int ID_NhomNhan { set; get; }
        public List<int> ID_NhanVienDuocGiao { get; set; }
        public string TenCongViec { set; get; }
        public string NoiDung { set; get; }
        public string DiaDiemDi { set; get; }
        public string DiaDiemDen { set; get; }
        public bool BatBuocCheckin { set; get; }
        public bool BatBuocChupAnh { set; get; }
        public bool BatBuocDeadline { set; get; }
        public float SoTien { set; get; }
        public float SoTienHang { set; get; }
        public int SoNguoiNhan { set; get; }
        public int TrangThai { set; get; }
        public DateTime NgayTao { set; get; }
        public DateTime NgayHetHan { set; get; }
        public DateTime NgayNhan { set; get; }
        public DateTime NgayHoanThanh { set; get; }

        public string TenNguoiGui { set; get; }
        public string NguoiDuocGiao { get; set; }
        public string TenNhom { set; get; }
        public string TenTrangThai { set; get; }
        public string MaMauTrangThai { set; get; }

        public double KinhDoDiemDi { set; get; }
        public double KinhDoDiemDen { set; get; }
        public double ViDoDiemDi { set; get; }
        public double ViDoDiemDen { set; get; }

        public string FileGiaoViec { get; set; }
    }
}