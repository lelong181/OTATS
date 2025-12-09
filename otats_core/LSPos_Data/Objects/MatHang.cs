using BusinessLayer.Model.Sell;
using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHang
/// </summary>
public class MatHang
{
    public int IDQLLH { get; set; }
    public int IDMatHang { get; set; }
    public string MaHang { get; set; }
    public string TenHang { get; set; }
    public double? GiaBuon { get; set; }
    public double? GiaLe { get; set; }
    public double? LoiNhuan { get; set; }
    public int IDDonVi { get; set; }
    public string TenDonVi { get; set; }
    public string KhuyenMai { get; set; }
    public double SoLuong { get; set; }
    public int ID_DANHMUC { get; set; }
    public string TenDanhMuc { get; set; }
    public string GhiChuGia { get; set; }
    public string MoTa { get; set; }
    public string MoTaNgan { get; set; }
    public string LinkGioiThieu { get; set; }
    public int ID_NhaCungCap { get; set; }
    public int ID_NhanHieu { get; set; }
    public double SoLuongTon { get; set; }
    public double SoLuongDieuChuyenKho { get; set; }
    public string AnhDaiDien { get; set; }
    public int IsDichVu { get; set; }
    public List<HangHoaAlbumOBJ> DanhSachAnh { get; set; }
    public List<HangHoa_DichVuModel> lstDichVu { get; set; }
    public ServiceRateModel serviceRateModel { get; set; }
    public MatHang()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}