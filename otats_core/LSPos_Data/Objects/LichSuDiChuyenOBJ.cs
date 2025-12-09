using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LichSuDiChuyenOBJ
/// </summary>
public class LichSuDiChuyenOBJ
{
	public LichSuDiChuyenOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string nhanvien { get; set; }
    public string tinhtrangpin { get; set; }
    public DateTime thoigian { get; set; }
    public double kinhdo { get; set; }
    public double vido { get; set; }
    public string ghichu { get; set; }
    public double accuracy { get; set; }
    public string tenkhachhang { get; set; }
    public string diachikhachhang { get; set; }
    public int idkhachhang { get; set; }
    public int idnhanvien { get; set; }
    public DateTime thoigiantaidiem { get; set; }
    public DateTime thoigianvaodiem { get; set; }
    public DateTime thoigianradiem { get; set; }
    public DateTime thoigianketthuc { get; set; }
    public int type { get; set; }
    public double speed { get; set; }
}
