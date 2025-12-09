using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CTKM_OBJ
{
    public CTKM_OBJ()
	{
	}

    public int idctkm { get; set; }
    public int idct { get; set; }
    public int idnhanvien { get; set; }
    public int idquanly { get; set; }
    public int loai { get; set; }
    public double chietkhauphantram  { get; set; }
    public double chietkhautien  { get; set; } 

    public string tenctkm { get; set; }
    public string ghichu { get; set; }
    public DateTime ngayapdung { get; set; }
    public DateTime ngaytao { get; set; }
    public DateTime ngayketthuc { get; set; }
    public int trangthai { get; set; }
    public int hethan { get; set; }
    public List<ChiTietCTKMOBJ> chitietkhuyenmai { get; set; }
    public int dachon { get; set; }
}

public class ChiTietCTKMOBJ
{
    public ChiTietCTKMOBJ() { }

    public int id { get; set; }
    public int idctkm { get; set; }
    public int idhang { get; set; }
    public double chietkhauphantram_banle { get; set; }
    public double chietkhautien_banle { get; set; }
    public double chietkhauphantram_banbuon { get; set; }
    public double chietkhautien_banbuon { get; set; }
    public string ghichu { get; set; }

    public string mahang { get; set; }
    public string tenhang { get; set; }
    public string donvi { get; set; }
    public string giabuon { get; set; }
    public string giale { get; set; }
    
}

public class ChiTietCTKMMatHang_OBJ
{
    public ChiTietCTKMMatHang_OBJ() { }
    public int id { get; set; }
    public int idctkm { get; set; }
    public int loai { get; set; }
    public string tenctkm { get; set; }
    public string anhdaidienctkm { get; set; }
    public DateTime ngayapdung { get; set; }
    public DateTime ngaytao { get; set; }
    public DateTime ngayketthuc { get; set; }
    public int trangthai { get; set; }
    public int idhang { get; set; }

    public double chietkhauphantram_banle { get; set; }
    public double chietkhautien_banle { get; set; }
    public double chietkhauphantram_banbuon { get; set; }
    public double chietkhautien_banbuon { get; set; }
    public string ghichu { get; set; }

    public string mahang { get; set; }
    public string tenhang { get; set; }
    public string donvi { get; set; }
    public string giabuon { get; set; }
    public string giale { get; set; }

    public double SoLuongDatKM_Den { get; set; }
    public double SoLuongDatKM_Tu { get; set; }
    public double TongTienDatKM_Den { get; set; }
    public double TongTienDatKM_Tu { get; set; }

    public int ApDungBoiSo { get; set; }

    public List<ChiTietHangTang_OBJ_v2> chitiethangtang { get; set; }
}

public class ChiTietHangTang_OBJ_v2
{
    public int idchitiethangtang { get; set; }
    public int idchitietctkm { get; set; }
    public int idhanghoa { get; set; }
    public string tenhang { get; set; }
    public string mahang { get; set; }
    public string giabanbuon { get; set; }
    public string giabanle { get; set; }
    public int soluong { get; set; }
    public string tongtien { get; set; }
}
