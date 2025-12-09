using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoaiKhachHangOBJ
/// </summary>
public class CheckListOBJ
{
    public CheckListOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_CheckList { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_KhachHang { get; set; }
    public string TenCheckList { get; set; }
    public int STT { get; set; }
    public int DaCheck { get; set; }
    public int TrangThai { get; set; }
    public string TenTrangThai { get; set; }

    public DateTime ThoiGian { get; set; }
}