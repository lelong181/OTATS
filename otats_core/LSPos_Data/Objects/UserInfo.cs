using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserInfo
/// </summary>
public class UserInfo
{
    public int ID_QuanLy { get; set; }
    
    public string Username { get; set; }
    public string TenAdmin { get; set; }
    public int ID_QLLH { get; set; }
    public int Level { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsHDV { get; set; }
    public int ID_Cha { get; set; }
    public string DanhSachNhom { get; set; }
    public int CapDo { get; set; }
    public string logo { get; set; }
    public int XuatLoTrinhDiaChi { get; set; }

    public int QuyenNhapChiTietMatHang { get; set; }

    public string MauInDonHang { get; set; }
    public int XuatDanhSachDonHangTheoMau { get; set; }

    public string DinhDang_NgayHienThi { get; set; }
    public int DinhDangTienSoThapPhan { get; set; }
    public int CheckTonKhiLapDonHang { get; set; }

    public string Email { get; set; }
    public string Phone { get; set; }
    public UserInfo(int idql, string username, int id_qllh, int capquanly, string ten, bool isAdmin, int idCha, string dsNhom,int _CapDo,string _logo,int _XuatLoTrinhDiaChi, int _QuyenNhapChiTietMatHang
        ,string _MauInDonHang, int _XuatDanhSachDonHangTheoMau, string _DinhDang_NgayHienThi, int _DinhDangTienSoThapPhan, int _CheckTonKhiLapDonHang)
	{
         
        ID_QuanLy = idql;
        Username = username;
        ID_QLLH = id_qllh;
        Level = capquanly;
        TenAdmin = ten;
        IsAdmin = isAdmin;
        ID_Cha = idCha;
        DanhSachNhom = dsNhom;
        CapDo = _CapDo;
        logo = _logo;
        XuatLoTrinhDiaChi = _XuatLoTrinhDiaChi;
        QuyenNhapChiTietMatHang = _QuyenNhapChiTietMatHang;
        MauInDonHang = _MauInDonHang;
        XuatDanhSachDonHangTheoMau = _XuatDanhSachDonHangTheoMau;

        DinhDang_NgayHienThi = _DinhDang_NgayHienThi;
        DinhDangTienSoThapPhan = _DinhDangTienSoThapPhan;
        CheckTonKhiLapDonHang = _CheckTonKhiLapDonHang;
    }

     
}