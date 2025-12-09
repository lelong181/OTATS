using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ChiTietMatHangDonHangModels
    {
        public long ID_ChiTiet_MatHang_DonHang { get; set; }
        public string MoTa { get; set; }
        public long ID_DonHang { get; set; }
        public long ID_MatHang { get; set; }
        public double ChieuDai { get; set; }
        public double ChieuRong { get; set; }
        public double ChieuCao { get; set; }
        public double SoLuong { get; set; }
        public double Giaban { get; set; }
        public double GiaInTrenVe { get; set; }
        public double TongTien { get; set; }
        public int ID_HangHoaDichVu { get; set; }
        public int ID_DichVu { get; set; }
        public string MaVeDichVu { get; set; }
        public string MaBookingDichVu { get; set; }
        public string MaDonHangDichVu { get; set; }
        public DateTime HanSuDung { get; set; }
        public string TenTrangThai { get; set; }
        public string NgayTao { get; set; }
        public string TenHienThi { get; set; }
        public string ServiceID { get; set; }
        public string MaVeBoSung { get; set; }
        public int TrangThai { get; set; }
        public string Icon { get; set; }
        public DateTime ThoiGianSuDung { get; set; }
        public string ACM { get; set; }
        public string KetQua { get; set; }
        public string GroupLink { get; set; }

    }
}