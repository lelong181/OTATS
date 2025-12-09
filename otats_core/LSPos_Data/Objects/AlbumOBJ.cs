using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ImagesOBJ
/// </summary>
public class AlbumOBJ
{
    public AlbumOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int idalbum { get; set; }
    public string hinhdaidien { get; set; }
    public double kinhdo { get; set; }
    public double vido { get; set; }
    public int idkhachhang { get; set; }
    public int idnhanvien { get; set; }
    public int idcongty { get; set; }
    public string ghichu { get; set; }
    public string thoigiantao { get; set; }
    public string diachi { get; set; }
    public string tennhanvien { get; set; }
    public string tenkhachhang { get; set; }
    public List<ImageOBJ> danhsachanh { get; set; }
}