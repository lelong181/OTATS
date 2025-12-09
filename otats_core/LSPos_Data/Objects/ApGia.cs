using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ApGia
/// </summary>
public class ApGia
{
    public int ID_ApGia { get; set; }
    public int ID_QuanLy { get; set; }
    public int ID_Hang { get; set; }
    public double GiaBanBuon { get; set; }
    public double GiaBanLe { get; set; }
    public DateTime TuNgay { get; set; }
    //public int ID_ApGia { get; set; }
    public int TrangThai { get; set; }

	public ApGia()
	{
        
	}
}