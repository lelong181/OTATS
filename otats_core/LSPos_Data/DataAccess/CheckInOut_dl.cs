using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CheckInOut_dl
/// </summary>
/// 

public class CheckIn
{
    public int ID_QLLH { get; set; }
    public int ID_CheckIn { get; set; }
    public int ID_NhanVien { get; set; }
    public string TenNhanVien { get; set; }
    public int ID_KhachHang { get; set; }
    public string TenKhachHang { get; set; }
    public string DiaChiKhachHang { get; set; }
    public string DiaChi { get; set; }
    public string DiaChiRaDiem { get; set; }
    public DateTime CheckInTime { get; set; }
    public double CI_KinhDo { get; set; }
    public double CI_ViDo { get; set; }
    public DateTime CheckOutTime { get; set; }
    public double CO_KinhDo { get; set; }
    public double CO_ViDo { get; set; }
    public string GhiChu { get; set; }
    public string SoDonHang { get; set; }
    public string SoAnhChup { get; set; }
    public string CheckList { get; set; }
    public int ID_KeHoachDiChuyen { get; set; }
    public DateTime ThoiGianCheckInDuKien { get; set; }
    public DateTime ThoiGian { get; set; }
    public int ID_Tuyen { get; set; }
    public string TenTuyen { get; set; }
    public string ThoiGianVaoDiemThucTe_HienThi
    {
        get
        {
            string hienthi = "";
            TimeSpan ts = DateTime.Now - CheckInTime;
            if(CheckInTime.Year > 1900 && ts.TotalSeconds < 60)
            {
                //< 1 phut
                hienthi = ts.Seconds + " giây trước";
            }
            else if (CheckInTime.Year > 1900 && ts.TotalSeconds < 60 * 60)
            {
                //< 1 tieng
                hienthi = ts.Minutes + " phút, " + ts.Seconds + " giây trước";
            }
            else if (CheckInTime.Year > 1900 && ts.TotalSeconds < 60 * 60 * 24)
            {
                hienthi = "Khoảng " + Math.Round( ts.TotalHours,0 ) + " giờ trước";
                //< 1 ngay
            }
            else
            {
                hienthi = CheckInTime.Day + " tháng " + CheckInTime.Month +   " lúc " + CheckInTime.ToString("HH:mm");
            }
            return hienthi;
        }

         
    }
    public string ThoiGianTaiDiem
    {
        get
        {
            string hienthi = "";
            TimeSpan ts = DateTime.Now - CheckInTime;
            if (CheckInTime.Year > 1900 && ts.TotalSeconds < 60)
            {
                //< 1 phut
                hienthi = ts.Seconds + " giây";
            }
            else if (CheckInTime.Year > 1900 && ts.TotalSeconds < 60 * 60)
            {
                //< 1 tieng
                hienthi = ts.Minutes + " phút, " + ts.Seconds + " giây";
            }
            else if (CheckInTime.Year > 1900 && ts.TotalSeconds < 60 * 60 * 24)
            {
                hienthi = "Khoảng " + Math.Round(ts.TotalHours, 0) + " giờ";
                //< 1 ngay
            }
            else
            {
                hienthi = CheckInTime.Day + " tháng " + CheckInTime.Month + " lúc " + CheckInTime.ToString("HH:mm");
            }
            return hienthi;
        }


    }
    public string ThoiGianVaoDiemKeHoach_HienThi
    {
        get
        {
            string hienthi = "";
            TimeSpan ts = DateTime.Now - ThoiGianCheckInDuKien;
            if (ThoiGianCheckInDuKien.Year > 1900 && ts.TotalSeconds < 60)
            {
                //< 1 phut
                hienthi = ts.Seconds + " giây trước";
            }
            else if (ThoiGianCheckInDuKien.Year > 1900 && ts.TotalSeconds < 60 * 60)
            {
                //< 1 tieng
                hienthi = ts.Minutes + " phút, " + ts.Seconds + " giây trước (" + ThoiGianCheckInDuKien.ToString("HH:mm") + ")";
            }
            else if (ThoiGianCheckInDuKien.Year > 1900 && ts.TotalSeconds < 60 * 60 * 24)
            {
                hienthi = "Khoảng " + Math.Round(ts.TotalHours, 0) + " giờ trước ("+ ThoiGianCheckInDuKien .ToString("HH:mm")+ ")";
                //< 1 ngay
            }
            else
            {
                hienthi = ThoiGianCheckInDuKien.Day + " tháng " + ThoiGianCheckInDuKien.Month + " lúc " + ThoiGianCheckInDuKien.ToString("HH:mm");
            }
            return hienthi;
        }


    }

}

public class CheckInOut_dl
{
    log4net.ILog log = log4net.LogManager.GetLogger(typeof(CheckInOut_dl));
    private SqlDataHelper helper;

	public CheckInOut_dl()
	{
		//
		// TODO: Add constructor logic here
		//
        helper = new SqlDataHelper();
	}

    public CheckIn GetCheckInFromDataRow(DataRow dr)
    {
        try
        {
            CheckIn ci = new CheckIn();
            ci.ID_QLLH = dr["ID_QLLH"] == DBNull.Value ? 0 : int.Parse(dr["ID_QLLH"].ToString());
            ci.ID_CheckIn = dr["IDCheckIn"] == DBNull.Value ? 0 : int.Parse(dr["IDCheckIn"].ToString());
          
            try
            {
                ci.ID_KhachHang = int.Parse(dr["MaKhachHang"].ToString());
                ci.TenKhachHang = dr["TenKhachHang"].ToString();
            }
            catch
            {
                ci.ID_KhachHang = 0;
                ci.TenKhachHang = "Không xác định";
            }
            
            ci.DiaChiKhachHang = dr.Table.Columns.Contains("DiaChiKhachHang") ? dr["DiaChiKhachHang"].ToString() : "";
            ci.DiaChi = dr["DiaChi"].ToString();
            ci.DiaChiRaDiem = dr["DiaChiRaDiem"].ToString();
            ci.ID_NhanVien = dr["MaNhanVien"] == DBNull.Value ? 0 : int.Parse(dr["MaNhanVien"].ToString());
            
            ci.TenNhanVien = dr["TenNhanVien"].ToString();
            try
            {
                ci.CheckInTime = DateTime.Parse(dr["CheckInTime"].ToString());
                ci.CI_KinhDo = (dr["CI_KinhDo"].ToString() == "") ? 0 : double.Parse(dr["CI_KinhDo"].ToString());
                ci.CI_ViDo = (dr["CI_ViDo"].ToString() == "") ? 0 : double.Parse(dr["CI_ViDo"].ToString());
                ci.CheckOutTime = (dr["CheckOutTime"].ToString() == "") ? DateTime.MinValue : DateTime.Parse(dr["CheckOutTime"].ToString());
                ci.CO_KinhDo = (dr["CO_KinhDo"].ToString() == "") ? 0 : double.Parse(dr["CO_KinhDo"].ToString());
                ci.CO_ViDo = (dr["CO_ViDo"].ToString() == "") ? 0 : double.Parse(dr["CO_ViDo"].ToString());
            }
            catch (Exception)
            {

                
            }
            
          
            ci.GhiChu = dr["GhiChu"].ToString();
            ci.SoDonHang = dr["SoDonHang"].ToString();
            try
            {
                ci.ID_KeHoachDiChuyen = (dr["ID_KeHoachDiChuyen"].ToString() == "") ? 0 : int.Parse(dr["ID_KeHoachDiChuyen"].ToString());
                ci.ThoiGianCheckInDuKien = (dr["ThoiGianCheckInDuKien"].ToString() == "") ? DateTime.MinValue : DateTime.Parse(dr["ThoiGianCheckInDuKien"].ToString());
            }
            catch (Exception)
            {

                 
            }

            try
            {
                ci.ThoiGian = (dr["ThoiGian"].ToString() == "") ? DateTime.MinValue : DateTime.Parse(dr["ThoiGian"].ToString());
            }
            catch (Exception)
            {


            }
            try
            {
                ci.ID_Tuyen = (dr["ID_Tuyen"].ToString() == "") ? 0 : int.Parse(dr["ID_Tuyen"].ToString());
                ci.TenTuyen = dr["TenTuyen"].ToString();
            }
            catch (Exception)
            {


            }
            try
            {
                ci.SoAnhChup = dr["SoAnhChup"].ToString();
                ci.CheckList = dr["CheckList"].ToString();
            }
            catch (Exception)
            {


            }


            return ci;
        }
        catch
        {
            return null;
        }
    }

    public List<CheckIn> GetCheckInTheoIDNV(int IDQLLH,int ID_Nhom, int IDNV, DateTime TuNgay, DateTime DenNgay,int ID_QuanLy,int ID_KhachHang, int type, int ID_LoaiKhachHang)
    {
        SqlParameter[] pars = new SqlParameter[] {
              new SqlParameter("ID_Nhom", ID_Nhom),
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", IDNV),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("Loai", type),// : check in , 1 : check out,
            new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang) 
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_BaoCaoVaoRaDiem_v3", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        List<CheckIn> dsci = new List<CheckIn>();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                CheckIn nv = GetCheckInFromDataRow(dr);
                dsci.Add(nv);
            }
            catch
            {
            }
        }

        return dsci;
    }

    public List<CheckIn> GetCheckInById(int IdCheckIn)
    {
        SqlParameter[] pars = new SqlParameter[] {
      
            new SqlParameter("id", IdCheckIn) 
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_BaoCaoVaoRaDiem_ById", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        List<CheckIn> dsci = new List<CheckIn>();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                CheckIn nv = GetCheckInFromDataRow(dr);
                dsci.Add(nv);
            }
            catch
            {
            }
        }

        return dsci;
    }
    public  CheckIn GetCheckInMoiNhat_TheoIDNV(int IDNV)
    {
        SqlParameter[] pars = new SqlParameter[] {
             
            new SqlParameter("ID_NhanVien", IDNV),
             
        };

        DataSet ds = helper.ExecuteDataSet("sp_CheckIn_LayDiemDangCheckIn", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        CheckIn nv = new CheckIn();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                  nv = GetCheckInFromDataRow(dr);
                
            }
            catch
            {
            }
        }

        return nv;
    }
    public CheckIn GetCheckInMoiNhat_TheoIDNV_Latest(int IDNV)
    {
        SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_NhanVien", IDNV),

        };

        DataSet ds = helper.ExecuteDataSet("sp_CheckIn_LayDiemDangCheckIn_Latest", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        CheckIn nv = new CheckIn();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                nv = GetCheckInFromDataRow(dr);

            }
            catch
            {
            }
        }

        return nv;
    }

    public List<CheckIn> GetCheckInTheoIDNV(int IDQLLH,  int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
             
        };

        DataSet ds = helper.ExecuteDataSet("sp_CheckIn_GetDSVaoDiem_v2", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        List<CheckIn> dsci = new List<CheckIn>();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                CheckIn nv = GetCheckInFromDataRow(dr);
                dsci.Add(nv);
            }
            catch
            {
            }
        }

        return dsci;
    }


    public List<CheckIn> GetCheckInTheoIDNV_Noti(int IDQLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)

        };

        DataSet ds = helper.ExecuteDataSet("sp_CheckIn_GetDSVaoDiem_v2_Noti", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        List<CheckIn> dsci = new List<CheckIn>();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                CheckIn nv = GetCheckInFromDataRow(dr);
                dsci.Add(nv);
            }
            catch
            {
            }
        }

        return dsci;
    }


    public List<CheckIn> GetCheckIn_SapToiGio_TheoIDNV(int IDQLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)

        };

        DataSet ds = helper.ExecuteDataSet("sp_CheckIn_GetDSSapToGioVaoDiem", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        List<CheckIn> dsci = new List<CheckIn>();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                CheckIn nv = GetCheckInFromDataRow(dr);
                dsci.Add(nv);
            }
            catch
            {
            }
        }

        return dsci;
    }
    public List<CheckIn> GetCheckInDenGio_TheoIDNV(int IDQLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)

        };

        DataSet ds = helper.ExecuteDataSet("sp_CheckIn_GetDSDenGioVaoDiem", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;


        List<CheckIn> dsci = new List<CheckIn>();
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                CheckIn nv = GetCheckInFromDataRow(dr);
                dsci.Add(nv);
            }
            catch
            {
            }
        }

        return dsci;
    }

    public DataTable BaoCaoTongHopCheckInTheoKhachHang(int IDQLLH, int IDNV, DateTime TuNgay, DateTime DenNgay, int ID_QuanLy, int ID_KhachHang)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", IDNV),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("ID_KhachHang", ID_KhachHang)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_BaoCaoTongHopVaoDiemTheoKhachHang", pars);
        DataTable dt = ds.Tables[0];

        return dt;
    }


    public DataTable GetCheckInById_New(int IdCheckIn)
    {
        DataTable dt = null;
           SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("id", IdCheckIn)
        };

        try
        {
            DataSet ds = helper.ExecuteDataSet("sp_QL_BaoCaoVaoRaDiem_ById_new", pars);
              dt = ds.Tables[0];

        }
        catch (Exception ex)
        {

            log.Error(ex);
        }
       

        return dt;
    }

    public DataTable GetCheckInTheoIDNV_New(int IDQLLH, int ID_Nhom, int IDNV, DateTime TuNgay, DateTime DenNgay, int ID_QuanLy, int ID_KhachHang, int type, int ID_LoaiKhachHang)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
              new SqlParameter("ID_Nhom", ID_Nhom),
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", IDNV),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("Loai", type),// : check in , 1 : check out,
            new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_BaoCaoVaoRaDiem_v4", pars);
              dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

     

        return dt;
    }
}