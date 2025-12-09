using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoaiKhachHangOBJ
/// </summary>
public class LoaiKhachHangOBJ
{
    public LoaiKhachHangOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_LoaiKhachHang { get; set; }
    public int ID_QLLH { get; set; }
    public string TenLoaiKhachHang { get; set; }
    public string IconHienThi { get; set; }
    public DateTime NgayTao { get; set; }

}