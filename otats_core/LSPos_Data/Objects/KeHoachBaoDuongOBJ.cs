using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachOBJ
/// </summary>
public class KeHoachBaoDuongOBJ
{
	public KeHoachBaoDuongOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}
   
    public int ID_Xe_KeHoachBaoDuong { get; set; }
    public int ID_Xe { get; set; }
    public int ID_NhanVien { get; set; }

    public DateTime NgayBaoDuongDuKien { get; set; }
    public DateTime NgayBaoDuong { get; set; }

    public DateTime NgayBDTiepTheo { get; set; }
    public string BienKiemSoat { get; set; }
    public string TenNhanVien { get; set; }
    public string LoaiXe { get; set; }
    public int SoCho { get; set; }
    public string NoiDung { get; set; }
    public int TrangThai { get; set; }
    public string DiaDiemBaoDuong { get; set; }
    public string DiaChiBaoDuong { get; set; }
    public double KinhDo { get; set; }
    public double ChiPhi { get; set; }
    public double ViDo { get; set; }
    public string text_color { get; set; }
    public string text_color_mota { get; set; }


}