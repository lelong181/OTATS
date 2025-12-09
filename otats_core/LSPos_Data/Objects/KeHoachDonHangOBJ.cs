using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachDonHangOBJ
/// </summary>
public class KeHoachDonHangOBJ
{
	public KeHoachDonHangOBJ()
	{
		//
		// TODO: Add constructor logic here
		//
	}
     
    public double KinhDo { get; set; }
    public double ViDo { get; set; }
    public int ID_DonHang { get; set; }
    public int ID_KhachHang { get; set; }
    public int ID_NhanVien { get; set; }
    public string TenKhachHang { get; set; }
    public string TenNhanVien { get; set; }
    public int STT { get; set; }
    public string ThoiGianDuKien_Text { get; set; }
    public string GhiChu { get; set; }
    public int TrangThai { get; set; }
    public int IDKeHoach { get; set; }
    public DateTime ThoiGianDuKien { get; set; }
    public DateTime NgayTaoDonHang { get; set; }
    public DateTime ThoiGianThucTe { get; set; }
    public int IdQuanLy { get; set; }
    public string text_color { get; set; }
    public string text_color_mota { get; set; }

    public string MaThamChieu { get; set; }
}