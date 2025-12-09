using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHangOBJ
/// </summary>
public class HangHoaOBJ
{
    public HangHoaOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int idhang { get; set; }

    public string mahang { get; set; }

    public string tenhang { get; set; }

    public string giabuon { get; set; }

    public string giale { get; set; }

    public string donvi { get; set; }

    public double tonkho { get; set; } // sau order

    public double soluong { get; set; } //thuc te

    public string khuyenmai { get; set; }

    public int tonggiao { get; set; }

    public int hinhthucban { get; set; }

    public int iddanhmuc { get; set; }
    public string tendanhmuc { get; set; }

    //public List<ChiTietCTKMMatHangOBJ> ctkm { get; set; }
}