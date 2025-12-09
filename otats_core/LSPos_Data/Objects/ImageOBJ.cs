using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ImageOBJ
/// </summary>
public class ImageOBJ
{
	public ImageOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int imageid { get; set; }
    public string path { get; set; }
    public DateTime thoigian { get; set; }
     
    public double kinhdo { get; set; }
    public double vido { get; set; }
    public int idkhachhang { get; set; }
    public int idnhanvien { get; set; }
    public int idcongty { get; set; }
    public string ghichu { get; set; }
     
    public string tendaily { get; set; }
    public string tennhanvien { get; set; }
    public string diachi { get; set; }
    public int idalbum { get; set; }
    public int stt { get; set; }
    public int stt_identity { get; set; }

    public string path_thumbnail_medium { get; set; }

    public string path_thumbnail_small { get; set; }
}