using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyOBJ
/// </summary>
public class NhomOBJ
{
    public NhomOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_QLLH { set; get; }
    public int ID_Nhom { set; get; }
    public int ID_PARENT { set; get; }
    public DateTime NgayTao { set; get; }
    public int TrangThai { set; get; }
    public string TenNhom { get; set; }
    public string MaNhom { get; set; }
    public string SiteCode { get; set; }
    public float CongNoGioiHan { get; set; }
    public string TenHienThi_NhanVien { get; set; }
    public string TenHienThi_QuanLy { get; set; }
    public int SoLuongNhanVien { set; get; }
    public int SoLuongQuanLy { set; get; }
}