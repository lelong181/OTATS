using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HangHoaAlbumOBJ
/// </summary>
public class HangHoaAlbumOBJ
{
    public HangHoaAlbumOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int ID_AlbumAnh { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_QuanLy { get; set; }
    public string GhiChu { get; set; }
    public int ID_Hang { get; set; }
     
    public List<HangHoaImagesOBJ> DanhSachAnh { get; set; }
     
}