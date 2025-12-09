using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoaiKhachHangOBJ
/// </summary>
public class LichSuPhanHoi
{
    public LichSuPhanHoi()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_PhanHoi { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_NhanVien { get; set; }
    public int ID_KhachHang { get; set; }
    public int idcheckin { get; set; }
    public string ChiTiet { get; set; }
    public DateTime ThoiGian { get; set; }
    public string TenPhanHoi { get; set; }
    public string TenKhachHang { get; set; }
}