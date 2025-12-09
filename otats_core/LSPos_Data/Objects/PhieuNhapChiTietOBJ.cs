using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHangOBJ
/// </summary>
public class PhieuNhapChiTietOBJ
{
    public PhieuNhapChiTietOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int ID_ChiTietPhieuNhap { get; set; }
    public int ID_HangHoa { get; set; }
    public int ID_Kho { get; set; }

    public int ID_PhieuNhap { get; set; }
    public double SoLuong { get; set; }
    public double DonGia { get; set; }
    public double ThanhTien { get; set; }

}