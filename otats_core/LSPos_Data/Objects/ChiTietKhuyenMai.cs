using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ChiTietKhuyenMai
/// </summary>
public class ChiTietKhuyenMai
{
    public int ID_ChiTietCTKM { get; set; }
    public int ID_CTKM { get; set; }
    public int ID_Hang { get; set; }
    public string TenCTKM { get; set; }
    public string TenMatHang { get; set; }
    public float ChietKhauPhanTram_BanLe { get; set; }
    public float ChietKhauPhanTram_BanBuon{ get; set; }
    public string GhiChu { get; set; }
    public double ChietKhauTien_BanLe { get; set; }
    public double ChietKhauTien_BanBuon { get; set; }
    public double GiaBanBuon { get; set; }
    public double GiaBanLe{ get; set; }
    public string MaHang { get; set; }
    public string DonViTinh { get; set; }
    public DateTime NgayTao { get; set; }
    public int ID_DANHMUC { get; set; }
    public string TenDanhMuc { get; set; }
    public double GhiChuGia { get; set; }

    public double SoLuongDatKM_Tu { get; set; }
    public double SoLuongDatKM_Den { get; set; }
    public double TongTienDatKM_Tu { get; set; }
    public double TongTienDatKM_Den { get; set; }
    public int ApDungBoiSo { get; set; }

    public ChiTietKhuyenMai()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}