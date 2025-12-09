using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHangOBJ
/// </summary>
public class PhieuNhapOBJ
{
    public PhieuNhapOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int ID_PhieuNhap { get; set; }

    public int ID_QLLH { get; set; }
    public int ID_QuanLy { get; set; }
    public DateTime NgayNhap { get; set; }
    public string NoiDung { get; set; }

    public int ID_Kho { get; set; }
    public List<PhieuNhapChiTietOBJ> ChiTiet { get; set; }
}

public class PhieuDieuChinhOBJ
{
    public PhieuDieuChinhOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_PhieuDieuChinh { get; set; }
    public int ID_PhieuNhap { get; set; }

    public int ID_QLLH { get; set; }
    public int ID_QuanLy { get; set; }
    public DateTime NgayDieuChinh { get; set; }
    public string LyDoDieuChinh { get; set; }

    public List<PhieuDieuChinh_ChiTietOBJ> ChiTiet { get; set; }
}

public class PhieuDieuChinh_ChiTietOBJ
{
    public PhieuDieuChinh_ChiTietOBJ()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ID_PhieuDieuChinh_ChiTiet { get; set; }
    public int ID_PhieuDieuChinh { get; set; }

    public int ID_HangHoa { get; set; }
    public double SoLuong { get; set; }
    public double DonGia { get; set; }
    public double ThanhTien { get; set; }

     
}