using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoaiKhachHangOBJ
/// </summary>
public class NhanHieuOBJ
{
    public NhanHieuOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_NhanHieu { get; set; }
    public int ID_QLLH { get; set; }
    public string TenNhanHieu { get; set; }
    public int TrangThai { get; set; }
    public string TenTrangThai { get; set; }
    public DateTime NgayTao { get; set; }

}