using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyOBJ
/// </summary>
public class TinNhanOBJ
{
    public TinNhanOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_QLLH { set; get; }
    public int ID_TINNHAN { set; get; }
    public int ID_NHANVIEN { set; get; }
    public int ID_QUANLY { set; get; }
    public DateTime NgayGui { set; get; }
    public DateTime NgayXem { set; get; }
    public string NgayXemHienThi { set; get; }
    public string NgayGuiHienThi { set; get; }
    public int TrangThai { set; get; }
    public string TrangThaiHienThi { set; get; }
    public string NoiDung { get; set; }
    public string TenNhanVien { get; set; }
    public string TenQuanLy { get; set; }
    public string LoaiGui { get; set; }
    public int TypeSend { set; get; }
    
}