using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachDiChuyen
/// </summary>
public class KeHoachDiChuyenObj
{
	public KeHoachDiChuyenObj()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int idKeHoach;

    public int IDKeHoach
    {
        get { return idKeHoach; }
        set { idKeHoach = value; }
    }

    private int idNhanVien;

    public int IDNhanVien
    {
        get { return idNhanVien; }
        set { idNhanVien = value; }
    }

    private string tenNhanVien;

    public string TenNhanVien
    {
        get { return tenNhanVien; }
        set { tenNhanVien = value; }
    }

    private int idKhachHang;

    public int IDKhachHang
    {
        get { return idKhachHang; }
        set { idKhachHang = value; }
    }

    private string tenKhachHang;

    public string TenKhachHang
    {
        get { return tenKhachHang; }
        set { tenKhachHang = value; }
    }

    private string diaChi;

    public string DiaChi
    {
        get { return diaChi; }
        set { diaChi = value; }
    }
    private string duongPho;

    public string DuongPho
    {
        get { return duongPho; }
        set { duongPho = value; }
    }

    private DateTime thoiGianCheckInDuKien;

    public DateTime ThoiGianCheckInDuKien
    {
        get { return thoiGianCheckInDuKien; }
        set { thoiGianCheckInDuKien = value; }
    }

    private DateTime thoiGianCheckOutDuKien;

    public DateTime ThoiGianCheckOutDuKien
    {
        get { return thoiGianCheckOutDuKien; }
        set { thoiGianCheckOutDuKien = value; }
    }

    private DateTime thoiGianCheckInThucTe;

    public DateTime ThoiGianCheckInThucTe
    {
        get { return thoiGianCheckInThucTe; }
        set { thoiGianCheckInThucTe = value; }
    }

    private DateTime thoiGianCheckOutThucTe;

    public DateTime ThoiGianCheckOutThucTe
    {
        get { return thoiGianCheckOutThucTe; }
        set { thoiGianCheckOutThucTe = value; }
    }

    private int trangThai;

    public int TrangThai
    {
        get { return trangThai; }
        set { trangThai = value; }
    }

    private int thuTuCheckIn;

    public int ThuTuCheckIn
    {
        get { return thuTuCheckIn; }
        set { thuTuCheckIn = value; }
    }

    private string ghiChu;

    public string GhiChu
    {
        get { return ghiChu; }
        set { ghiChu = value; }
    }

    private int nguoiTao;

    public int NguoiTao
    {
        get { return nguoiTao; }
        set { nguoiTao = value; }
    }

    private DateTime ngayTao;

    public DateTime NgayTao
    {
        get { return ngayTao; }
        set { ngayTao = value; }
    }
    public string ViecCanLam { get; set; }
    public double KinhDo { get; set; }
    public double ViDo { get; set; }
    public string text_color { get; set; }
    public string text_color_mota { get; set; }
}