using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonVi
/// </summary>
public class ChiTieuKPIOBJ
{
    public int ID_ChiTieuKPI { get; set; }
   
    public int IDQLLH { get; set; }
    public int ID_NhanVien { get; set; }
    public double DoanhSo { get; set; }
    public double NgayCong { get; set; }
    public double LuotViengTham { get; set; }
    public double SoDonHang { get; set; }
    public DateTime ApDung_TuNgay { get; set; }
    public int TrangThai { get; set; }
    public ChiTieuKPIOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}