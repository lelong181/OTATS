using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonHangOBJ
/// </summary>
public class DonHangV2OBJ
{
	public DonHangV2OBJ()
	{
	}

    public string mathamchieu { get; set; }

    public int iddonhang { get; set; }

    public int idct { get; set; }

    public int idnhanvien { get; set; }

    public int idcuahang { get; set; }
    public int isProcess { get; set; }
    public double tongtien { get; set; }
    //public double DaGiao { get; set; }
    public string ghichu { get; set; }

    public DateTime thoigiantao { get; set; }
    public int trangthaigiaohang { get; set; }
    public int trangthaithanhtoan { get; set; }
    public int trangthaidonhang { get; set; }
    public List<ChiTietDonHangV2OBJ> chitietdonhang{get;set;}

    public double chietkhauphantram { get; set; }
    public double chietkhautien { get; set; }
    public double tongtienchietkhau { get; set; }
    public int idctkm { get; set; }
    public string tenctkm { get; set; }
    public int idcheckin { get; set; }
    public string macuahang { get; set; }
    public string tenkhachhang { get; set; }
    public double tiendathanhtoan { get; set; }
    public double phantramhaohut { get; set; }
    public double soluonghaohut { get; set; }
    public int idnhanvientao { get; set; }
    public double kinhdo { get; set; }
    public double vido { get; set; }
    public string diachitao { get; set; }
    public string diachixuathoadon { get; set; }
    public string sodienthoai { get; set; }
    public string diachikhachhang { get; set; }
    public List<AlbumOBJ> albumanh { get; set; }
    public int idkiemketonkhokh { get; set; }
    public List<CTKMOBJ> dskhuyenmai { get; set; }
    public List<ChiTietDonHangV2OBJ> chitiethangtang { get; set; }
}

  