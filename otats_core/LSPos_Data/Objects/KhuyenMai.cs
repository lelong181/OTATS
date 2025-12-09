using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KhuyenMai
/// </summary>
public class KhuyenMai
{

    public int ID_CTKM { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_QuanLy { get; set; }
    public int ID_NhanVien { get; set; }
    public string TenNhanVien { get; set; }
    public string TenQuanLy { get; set; }
    public string TenCTKM { get; set; }
    public DateTime NgayApDung { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public int Loai { get; set; }
    public float ChietKhauPhanTram { get; set; }
    public string GhiChu { get; set; }
    public double ChietKhauTien { get; set; }
    public int TrangThai { get; set; }
    public DateTime NgayTao { get; set; }
    public string AnhDaiDien { get; set; }
    public List<ChiTietKhuyenMai> ChiTietCTKM { get; set; }
    public AlbumOBJ DanhSachAnh { get; set; }
    public double TongTienDatKM_Tu { get; set; }
    public double TongTienDatKM_Den { get; set; }
    public string TenHinhThucKM { get; set; }
    public KhuyenMai()
	{

	}
}

public class ChiTietCTKMMatHangOBJ
{
    public ChiTietCTKMMatHangOBJ() { }
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
    public double giabuon { get; set; }
    public double giale { get; set; }

    public double SoLuongDatKM_Den { get; set; }
    public double SoLuongDatKM_Tu { get; set; }
    public double TongTienDatKM_Den { get; set; }
    public double TongTienDatKM_Tu { get; set; }
    public int ApDungBoiSo { get; set; }
    public List<ChiTietHangTangOBJ_v2> chitiethangtang { get; set; }
}

public class ChiTietHangTangOBJ_v2
{
    public int idchitiethangtang { get; set; }
    public int idchitietctkm { get; set; }
    public int idhanghoa { get; set; }
    public string tenhang { get; set; }
    public string mahang { get; set; }
    public double giabanbuon { get; set; }
    public double giabanle { get; set; }
    public double soluong { get; set; }
    public double tongtien { get; set; }
}
