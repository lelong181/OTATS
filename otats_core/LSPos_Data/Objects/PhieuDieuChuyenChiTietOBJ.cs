using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHangOBJ
/// </summary>
public class PhieuDieuChuyenChiTietOBJ
{
    public PhieuDieuChuyenChiTietOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    
    public int ID_ChiTietPhieuDieuChuyen { get; set; }
    public int ID_PhieuDieuChuyen { get; set; }
    public int ID_HangHoa { get; set; }
    public int ID_KhoXuat { get; set; }
    public int ID_KhoNhap { get; set; }
    public double SoLuong { get; set; }
     
}