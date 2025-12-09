using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ExportNhanVienDTO
    {
        public ExportNhanVienDTO()
        {
            this.IDNV = 0;
            this.idNhom = 0;
            this.TenDangNhap = "";
            this.TenDayDu = "";
            this.DienThoai = "";
            this.TrangThaiTrucTuyen = "";
            this.KinhDo = 0;
            this.ViDo = 0;
            this.PhienBan = "";
            this.ThoiGianGuiBanTinCuoiCung = "";
            this.TenNhom = "";
            this.AnhDaiDien = "";
            this.AnhDaiDien_thumbnail_medium = "";
            this.AnhDaiDien_thumbnail_small = "";
        }
        public int IDNV { get; set; }
        public int idNhom { get; set; }
        public string TenDangNhap { get; set; }
        public string TenDayDu { get; set; }
        public string DienThoai { get; set; }
        public string TrangThaiTrucTuyen { get; set; }
        public double KinhDo { get; set; }
        public double ViDo { get; set; }
        public string PhienBan { get; set; }
        public string ThoiGianGuiBanTinCuoiCung { get; set; }
        public string TenNhom { get; set; }
        public string AnhDaiDien { get; set; }
        public string AnhDaiDien_thumbnail_medium { get; set; }
        public string AnhDaiDien_thumbnail_small { get; set; }
    }
}