using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachDiChuyen
/// </summary>
public class KeHoachDiChuyen_TuyenObj
{
    public KeHoachDiChuyen_TuyenObj()
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

    public int ID_DuongPho { get; set; }
    public int ID_Tinh { get; set; }
    public int ID_Quan { get; set; }
    public int ID_Phuong { get; set; }

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
}