using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Tinh
/// </summary>
public class Tinh
{
    public int ID_Tinh { get; set; }
    public int ID_KhuVuc { get; set; }
    public string TenTinh { get; set; }
    public Tinh()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}

/// <summary>
/// 
/// </summary>
public class Quan
{
    public int ID_Quan { get; set; }
    public string TenQuan { get; set; }
    public int ID_Tinh { get; set; }
    public Quan()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}

/// <summary>
/// 
/// </summary>
public class Phuong
{
    public int ID_Phuong { get; set; }
    public string TenPhuong { get; set; }
    public int ID_Quan { get; set; }
    public Phuong()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}
public class DuongPho
{
    public int ID_DuongPho { get; set; }
    public string TenDuongPho { get; set; }
     
    public DuongPho()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}