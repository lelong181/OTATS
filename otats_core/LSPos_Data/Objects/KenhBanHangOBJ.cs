using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoaiKhachHangOBJ
/// </summary>
public class KenhBanHangOBJ
{
    public KenhBanHangOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_KenhBanHang { get; set; }
    public int ID_QLLH { get; set; }
    public string TenKenhBanHang { get; set; }
    public string MaKenhBanHang { get; set; }
    public DateTime NgayTao { get; set; }
    public int ID_KenhCapTren { get; set; }
    public int TenKenhCapTren { get; set; }
    public int TrangThai { get; set; }
}