using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Tinh
/// </summary>
public class XeOBJ
{
    public int ID_Xe  { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_NhanVien { get; set; }
    public string BienKiemSoat   { get; set; }
    public string NamSanXuat { get; set; }
    public string SoCho { get; set; }
    public string LoaiXe { get; set; }
    public string MoTa { get; set; }
    
    public int ChuKyBaoDuong { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime NgayBDGanNhat { get; set; }
    public DateTime NgayBDTiepTheo { get; set; }
    public XeOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}

/// <summary>
/// 
/// </summary>
 