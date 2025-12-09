using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KhachHang
/// </summary>
public class KhachHang
{
    public int IDKhachHang { get; set; }
    public int IDQLLH { get; set; }
    public int ID_NhanVien { get; set; }
    public string TenNhanVien { get; set; }
    public string Ten { get; set; }
    public string MaKH { get; set; }
    public string TenDayDu { get; set; }
    public string DiaChi { get; set; }
    public int ID_Tinh { get; set; }
    public int ID_KhuVuc { get; set; }
    public int ID_Quan { get; set; }
    public int ID_Phuong { get; set; }
    public string TenQuan { get; set; }
    public string TenTinh { get; set; }
    public string TenPhuong { get; set; }
    public string SoDienThoai { get; set; }
    public string NguoiLienHe { get; set; }
    public DateTime NgaySinh { get; set; }
    public string SoTKNganHang { get; set; }
    public string MaSoThue { get; set; }
    public string Email { get; set; }
    public string Fax { get; set; }
    public string Website { get; set; }
    public double KinhDo { get; set; }
    public double ViDo { get; set; }
    public string GhiChu { get; set; }
    public int diagioihanhchinhid { get; set; }
    public string TenVietTat { get; set; }
    public string NguoiDaiDien { get; set; }
    public string tinh { get; set; }
    public int TrangThai { get; set; }
    public string Imgurl { get; set; }
    public string Imgurl2 { get; set; }
    public string Imgurl3 { get; set; }
    public string Imgurl4 { get; set; }
    public DateTime NgayTao { get; set; }
    public string DuongPho { get; set; }

    public int ID_QuanLy { get; set; }
    public string SoDienThoai2 { get; set; }
    public string SoDienThoai3 { get; set; }
    public string SoDienThoaiMacDinh { get; set; }
    public string DiaChiXuatHoaDon { get; set; }

    public List<ImageOBJ> danhsachanh { get; set; }
    public int ID_LoaiKhachHang { get; set; }
    public string TenLoaiKhachHang { get; set; }
    public string TenKenhBanHang { get; set; }
    public string GhiChuKhiXoa { get; set; }
    public int ID_NhomKH { get; set; }
    public int ID_Cha { get; set; }

    public DateTime LastUpdate_ThoiGian_NhanVien { get; set; }
    public DateTime LastUpdate_ThoiGian_QuanLy { get; set; }
    public int LastUpdate_ID_NhanVien { get; set; }
    public int LastUpdate_ID_QuanLy { get; set; }

    public string LastUpdate_Ten_NhanVien { get; set; }
    public string LastUpdate_Ten_QuanLy { get; set; }

    public KhachHang()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}