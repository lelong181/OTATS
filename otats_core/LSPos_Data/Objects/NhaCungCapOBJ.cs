using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NhaCungCapOBJ
/// </summary>
public class NhaCungCapOBJ
{
    public NhaCungCapOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_NhaCungCap { get; set; }
    public int ID_QLLH { get; set; }
    public string TenNhaCungCap { get; set; }
    public string DiaChi { get; set; }
    public string NguoiLienHe { get; set; }
    public string DienThoaiLienHe { get; set; }
    public int TrangThai { get; set; }
    public string TenTrangThai { get; set; }
    public string ProfileID { get; set; }
    public string ProfileCode { get; set; }
    public string PaymentTypeID { get; set; }
    public string AccountReceivableNo { get; set; }
    public decimal AccountReceivableBalance { get; set; }
    public string SiteCode { get; set; }
    public DateTime NgayTao { get; set; }

}