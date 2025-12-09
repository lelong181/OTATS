using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHangOBJ
/// </summary>
public class PhieuDieuChuyenOBJ
{
    public PhieuDieuChuyenOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int ID_PhieuDieuChuyen { get; set; }

    public int ID_QLLH { get; set; }
    public int ID_QuanLy { get; set; }
    public int ID_KhoXuat { get; set; }
    public int ID_KhoNhap { get; set; }

    public DateTime NgayDieuChuyen { get; set; }
    public string DienGiai { get; set; }

    
    public List<PhieuDieuChuyenChiTietOBJ> ChiTiet { get; set; }
}