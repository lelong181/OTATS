using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NhanVien
/// </summary>
public class NhanVien
{
    public int IDNV { get; set; }
    public int IDQLLH { get; set; }
    public string TenDangNhap { get; set; }
    public string TenDayDu { get; set; }
    public string MatKhau { get; set; }
    public int TrangThai { get; set; }
    public DateTime ThoiGianHoatDong { get; set; }
    public DateTime ThoiGianGuiBanTinCuoiCung { get; set; }
    public DateTime ThoiGianDangXuat { get; set; }
    public int TrucTuyen { get; set; }
    public int TrangThaiKetNoi { get; set; }
    public string DiaChi { get; set; }
    public string QueQuan { get; set; }
    public DateTime NgaySinh { get; set; }
    public string Email { get; set; }
    public string DienThoai { get; set; }
    public int ID_QuanLy { get; set; }
    public string PhienBan { get; set; }
    public string HinhThucDangXuat { get; set; }
    public string TinhTrangPin { get; set; }
    public int ID_Nhom { get; set; }
    public string TenNhom { get; set; }
    public string TrangThaiTrucTuyen { get; set; }
    public string TrangThaiKetNoi_Text { get; set; }
    public string TenLoaiKetNoi { get; set; }
    public DateTime ThoiGianCapNhat { get; set; }
    public int TruongNhom { get; set; }
    public double KinhDo { get; set; }
    public double ViDo { get; set; }

    public string DongMay { get; set; }
    public string DoiMay { get; set; }
    public string TenMay { get; set; }
    public string Imei { get; set; }
    public string OSVersion { get; set; }
    public string OS  { get; set; }
    public  bool IsFakeGPS { get; set; }
    public bool isCheDoTietKiemPin { get; set; }
    public int TrangThaiXoa { get; set; }
    public DateTime NgayXoa { get; set; }
    public string AnhDaiDien { get; set; }
    public string AnhDaiDien_thumbnail_medium { get; set; }
    public string AnhDaiDien_thumbnail_small { get; set; }
    public string AppFakeGPS { get; set; }
    public int GioiTinh { get; set; }
    public int ID_ChucVu { get; set; }
    public string ChucVu { get; set; }
    public int ID_NhomKhachHang_MacDinh { get; set; }
    public DateTime LastUpdate_ThoiGian_NhanVien { get; set; }
    public DateTime LastUpdate_ThoiGian_QuanLy { get; set; }
    public int LastUpdate_ID_NhanVien { get; set; }
    public int LastUpdate_ID_QuanLy { get; set; }

    public string LastUpdate_Ten_NhanVien { get; set; }
    public string LastUpdate_Ten_QuanLy { get; set; }

    public NhanVien()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}