using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class KhachHangDTO
    {
        public int RowNum { get; set; }
        public List<ImageOBJ> AnhDaiDien { get; set; }
        public string Imgurl { get; set; }
        public int ID_QLLH { get; set; }
        public int ID_NhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public int ID_KhachHang { get; set; }
        public string MaKH { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public double KinhDo { get; set; }
        public double ViDo { get; set; }
        public string DuongPho { get; set; }
        public string TenKhuVuc { get; set; }
        public int ID_KhuVuc { get; set; }
        public string TenTinh { get; set; }
        public int ID_Tinh { get; set; }
        public string TenQuan { get; set; }
        public int ID_Quan { get; set; }
        public string TenPhuong { get; set; }
        public int ID_Phuong { get; set; }
        public string DienThoaiMacDinh { get; set; }
        public string DienThoai { get; set; }
        public string DienThoai2 { get; set; }
        public string DienThoai3 { get; set; }
        public string Fax { get; set; }
        public string TenLoaiKhachHang { get; set; }
        public int ID_LoaiKhachHang { get; set; }
        public int ID_NhomKH { get; set; }
        public string TenNhomKH { get; set; }
        public string NguoiLienHe { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DiaChiXuatHoaDon { get; set; }
        public string SoTKNganHang { get; set; }
        public string MaSoThue { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
        public int ID_KenhCapTren { get; set; }
        public string TenKenhCapTren { get; set; }
        public string ToaDoKhachHang { get; set; }
        public string ID_Quyen { get; set; }
    }
}