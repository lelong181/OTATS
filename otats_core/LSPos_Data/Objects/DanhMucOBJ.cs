using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyOBJ
/// </summary>
public class DanhMucOBJ
{
    public DanhMucOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_QLLH { set; get; }
    public int ID_DANHMUC { set; get; }
    public int ID_PARENT { set; get; }
    public DateTime NgayTao { set; get; }
    public int TrangThai { set; get; }
    public string TenDanhMuc { get; set; }
    public int SoLuongMatHang { get; set; }
    public int SoLuongDanhMucCon { get; set; }
    public string TenHienThi { get; set; }
    public string AnhDaiDien { get; set; }

    public int ID_Nhom { set; get; }
}