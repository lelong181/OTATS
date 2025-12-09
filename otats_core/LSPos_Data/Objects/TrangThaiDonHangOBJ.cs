using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TrangThaiDonHangOBJ
/// </summary>
public class TrangThaiDonHangOBJ
{
    public TrangThaiDonHangOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_TrangThaiDonHang { get; set; }
    public int ID_QLLH { get; set; }
    public string TenTrangThai { get; set; }
    public string MauTrangThai { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime NgayXoa { get; set; }
    public int TrangThaiXoa { get; set; }
    public int ID_QuanLyXoa { get; set; }
    public int MacDinh { get; set; }
    public int KetThuc { get; set; }
    public int GuiSMS { get; set; }
    public string SMSTemplate { get; set; }
    public int GuiEmail { get; set; }
    public string EmailTemplate { get; set; }
}