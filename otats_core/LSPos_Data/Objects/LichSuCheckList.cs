using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoaiKhachHangOBJ
/// </summary>
public class LichSuCheckList
{
    public LichSuCheckList()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_CheckList { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_NhanVien { get; set; }
    public int ID_KhachHang { get; set; }
    public string ChiTiet { get; set; }
    public int idcheckin { get; set; }
    public DateTime ThoiGian { get; set; }
    public string TenCheckList { get; set; }
    public string TenKhachHang { get; set; }
}