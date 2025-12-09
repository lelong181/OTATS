using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachDiChuyen_dl
/// </summary>
public class KeHoachDiChuyen_Tuyen_dl
{
    private SqlDataHelper helper;
    public KeHoachDiChuyen_Tuyen_dl()
    {
        helper = new SqlDataHelper();
    }
    log4net.ILog log = log4net.LogManager.GetLogger(typeof(KeHoachDiChuyen_Tuyen_dl));
    public KeHoachDiChuyen_TuyenObj GetKeHoachFromDataRow(DataRow dr)
    {
        try
        {
            KeHoachDiChuyen_TuyenObj keHoach = new KeHoachDiChuyen_TuyenObj();
            //
            try
            {
                keHoach.IDKeHoach = (dr["ID"] != DBNull.Value) ? int.Parse(dr["ID"].ToString()) : 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                
                 
            }
            keHoach.ID_DuongPho = (dr["ID_DuongPho"] != DBNull.Value) ? int.Parse(dr["ID_DuongPho"].ToString()) : 0;
            keHoach.ID_Quan = (dr["ID_Quan"] != DBNull.Value) ? int.Parse(dr["ID_Quan"].ToString()) : 0;
            keHoach.ID_Tinh = (dr["ID_Tinh"] != DBNull.Value) ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
            keHoach.ID_Phuong = (dr["ID_Phuong"] != DBNull.Value) ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
            keHoach.IDNhanVien = (dr["ID_NhanVien"] != DBNull.Value) ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
           
            keHoach.TenNhanVien = (dr["TenNhanVien"] != DBNull.Value) ? dr["TenNhanVien"].ToString() : "";
            keHoach.ThoiGianCheckInDuKien = Convert.ToDateTime(dr["ThoiGianCheckInDuKien"].ToString() != "" ? dr["ThoiGianCheckInDuKien"] : "1900-1-1");
            keHoach.ThoiGianCheckOutDuKien = Convert.ToDateTime(dr["ThoiGianCheckOutDuKien"].ToString() != "" ? dr["ThoiGianCheckOutDuKien"] : "1900-1-1");
            keHoach.ThoiGianCheckInThucTe = Convert.ToDateTime(dr["ThoiGianCheckInThucTe"].ToString() != "" ? dr["ThoiGianCheckInThucTe"] : "1900-1-1");
            keHoach.ThoiGianCheckOutThucTe = Convert.ToDateTime(dr["ThoiGianCheckOutThucTe"].ToString() != "" ? dr["ThoiGianCheckOutThucTe"] : "1900-1-1");
            
            keHoach.TrangThai = (dr["TrangThai"] != DBNull.Value) ? Convert.ToInt16(dr["TrangThai"]) : 0;
            keHoach.ThuTuCheckIn = (dr["ThuTuCheckIn"] != DBNull.Value) ? Convert.ToInt16(dr["ThuTuCheckIn"]) : 0;
            keHoach.GhiChu = (dr["GhiChu"] != DBNull.Value ? dr["GhiChu"] : "").ToString();
            keHoach.DuongPho = (dr["DuongPho"] != DBNull.Value ? dr["DuongPho"] : "").ToString();
            return keHoach;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

 

    //public DataTable GetKeHoachLastEdit(int ID_NhanVien, DateTime Ngay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay)
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_KeHoachGetLastEdit", pars);
    //    try
    //    {
    //        return ds.Tables[0];
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}
    public List<KeHoachDiChuyen_TuyenObj> GetKeHoachTheoNhanVien(int ID_QLLH, int ID_QuanLy, int ID_NhanVien, DateTime from, DateTime to)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
               new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", from),
            new SqlParameter("@DenNgay", to)
        };

        DataSet ds = helper.ExecuteDataSet("sp_KeHoachDiChuyen_Tuyen_GetKeHoach", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDiChuyen_TuyenObj> dskhdc = new List<KeHoachDiChuyen_TuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyen_TuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public KeHoachDiChuyen_TuyenObj GetKeHoachTuyenById( int ID_KeHoachTuyen)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_KeHoachTuyen", ID_KeHoachTuyen) 
        };
        KeHoachDiChuyen_TuyenObj khdc = new KeHoachDiChuyen_TuyenObj();
        DataSet ds = helper.ExecuteDataSet("sp_KeHoachDiChuyen_Tuyen_GetKeHoachTuyenById", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
             
            foreach (DataRow dr in dt.Rows)
            {
              khdc = GetKeHoachFromDataRow(dr);
                 
            }

            
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
        return khdc;
    }
    public List<KeHoachDiChuyen_TuyenObj> CheckKeHoachTheoNhanVien(int ID_NhanVien, DateTime Ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay)
        };

        DataSet ds = helper.ExecuteDataSet("sp_KeHoachDiChuyen_Tuyen_GetKeHoach", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDiChuyen_TuyenObj> dskhdc = new List<KeHoachDiChuyen_TuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyen_TuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public int XoaKeHoach(int ID_NhanVien, int ID_QuanLy, DateTime Ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };
        try
        {
            return helper.ExecuteNonQuery("sp_KeHoachDiChuyen_XoaKeHoach", pars);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return 0;
        }
    }
    public int ThemKeHoach(KeHoachDiChuyen_TuyenObj khdc)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", khdc.IDNhanVien),
            new SqlParameter("@ID_DuongPho", khdc.ID_DuongPho),
             new SqlParameter("@ID_Quan", khdc.ID_Quan),
            new SqlParameter("@ID_Tinh", khdc.ID_Tinh),
               new SqlParameter("@ID_Phuong", khdc.ID_Phuong),
            new SqlParameter("@DuongPho", khdc.DuongPho),
            new SqlParameter("@ThoiGianCheckInDuKien", khdc.ThoiGianCheckInDuKien),
            new SqlParameter("@ThoiGianCheckOutDuKien", khdc.ThoiGianCheckOutDuKien),
            new SqlParameter("@GhiChu", khdc.GhiChu)
        };
        try
        {
            if (khdc.ThoiGianCheckInDuKien < DateTime.Now)
            {
                return -1;
            }
            else
                return helper.ExecuteNonQuery("sp_KeHoachDiChuyen_Tuyen_ThemMoi", pars);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return 0;
        }
    }

    //public int DeleteKeHoach(KeHoachDiChuyenObj khdc)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", khdc.IDNhanVien),
    //        new SqlParameter("@ID_KhachHang", khdc.IDKhachHang),
    //        new SqlParameter("@ThoiGianCheckInDuKien", khdc.ThoiGianCheckInDuKien.Date)
    //    };
    //    try
    //    {
    //        return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachDiChuyen", pars);
    //    }
    //    catch
    //    {
    //        return 0;
    //    }
    //}

    //public int DeleteKeHoachTheoNgay(int ID_NhanVien, DateTime ngay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@ngay", ngay)
    //    };
    //    try
    //    {
    //        return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachTheoNgay", pars);
    //    }
    //    catch
    //    {
    //        return 0;
    //    }
    //}

    //public List<KeHoachDiChuyenObj> GetKeHoachTheoNhanVien_Moi(int ID_NhanVien, DateTime TuNgay, DateTime DenNgay, int ID_QLLH, int ID_QuanLy)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@TuNgay", TuNgay),
    //        new SqlParameter("@DenNgay", DenNgay),
    //        new SqlParameter("@ID_QLLH", ID_QLLH),
    //        new SqlParameter("@ID_QuanLy", ID_QuanLy)
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien_New", pars);
    //    DataTable dt = ds.Tables[0];

    //    if (dt.Rows.Count == 0)
    //        return null;

    //    try
    //    {
    //        List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
    //            dskhdc.Add(khdc);
    //        }

    //        return dskhdc;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}
}