using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LichHenOBJ
/// </summary>
public class LichHenOBJ
{
    public LichHenOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int ID_LichHen { set; get; }
    public int ID_QLLH { set; get; }
    public int ID_NhanVien { set; get; }
    public int ID_KhachHang { set; get; }
    public DateTime ThoiGian { set; get; }
    public DateTime ThoiGianCapNhat { set; get; }
    public string ThoiGian_HienThi { set; get; }
    public int TrangThai { set; get; }
    public string NoiDung { get; set; }
    public string KetQua { get; set; }
    public string TenNhanVien { get; set; }
    public string TenKhachHang { get; set; }
    public string DiaChi { get; set; }
}