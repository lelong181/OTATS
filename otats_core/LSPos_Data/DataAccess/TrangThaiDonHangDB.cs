using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class TrangThaiDonHangDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TrangThaiDonHangDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public TrangThaiDonHangDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public TrangThaiDonHangOBJ GetTrangThaiDonHangById(int ID_TrangThaiDonHang)
    {
        TrangThaiDonHangOBJ rs = new TrangThaiDonHangOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_TrangThaiDonHang", ID_TrangThaiDonHang)
        };

        DataSet ds = db.ExecuteDataSet("sp_TrangThaiDonHang_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_TrangThaiDonHang = int.Parse(dr["ID_TrangThaiDonHang"].ToString());
            rs.TenTrangThai = dr["TenTrangThai"].ToString();
            rs.MauTrangThai = dr["MauTrangThai"].ToString();
            rs.TrangThaiXoa = dr["TrangThaiXoa"].ToString() != "" ? int.Parse(dr["TrangThaiXoa"].ToString()) : 0;
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.NgayXoa = dr["NgayXoa"].ToString() != "" ? DateTime.Parse(dr["NgayXoa"].ToString()) : rs.NgayXoa;
            rs.ID_QuanLyXoa = dr["ID_QuanLyXoa"].ToString() != "" ? int.Parse(dr["ID_QuanLyXoa"].ToString()) : 0;
            rs.MacDinh = dr["MacDinh"].ToString() != "" ? int.Parse(dr["MacDinh"].ToString()) : 0;
            rs.KetThuc = dr["KetThuc"].ToString() != "" ? int.Parse(dr["KetThuc"].ToString()) : 0;
            rs.GuiSMS = dr["GuiSMS"].ToString() != "" ? int.Parse(dr["GuiSMS"].ToString()) : 0;
            rs.GuiEmail = dr["GuiEmail"].ToString() != "" ? int.Parse(dr["GuiEmail"].ToString()) : 0;
            rs.SMSTemplate = dr["SMSTemplate"].ToString();
            rs.EmailTemplate = dr["EmailTemplate"].ToString();
        }
        catch
        {
            return null;
        }
        return rs;
    }
    public TrangThaiDonHangOBJ CheckTrangThaiDonHang(int ID_QLLH, int KetThuc, int MacDinh)
    {
        TrangThaiDonHangOBJ rs = new TrangThaiDonHangOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
             new SqlParameter("KetThuc", KetThuc),
              new SqlParameter("MacDinh", MacDinh)
        };

        DataSet ds = db.ExecuteDataSet("sp_TrangThaiDonHang_CheckTonTai", pars);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            try
            {

                rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
                rs.ID_TrangThaiDonHang = int.Parse(dr["ID_TrangThaiDonHang"].ToString());
                rs.TenTrangThai = dr["TenTrangThai"].ToString();
                rs.MauTrangThai = dr["MauTrangThai"].ToString();
                rs.TrangThaiXoa = dr["TrangThaiXoa"].ToString() != "" ? int.Parse(dr["TrangThaiXoa"].ToString()) : 0;
                rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
                rs.NgayXoa = dr["NgayXoa"].ToString() != "" ? DateTime.Parse(dr["NgayXoa"].ToString()) : rs.NgayXoa;
                rs.ID_QuanLyXoa = dr["ID_QuanLyXoa"].ToString() != "" ? int.Parse(dr["ID_QuanLyXoa"].ToString()) : 0;
                rs.MacDinh = dr["MacDinh"].ToString() != "" ? int.Parse(dr["MacDinh"].ToString()) : 0;
                rs.KetThuc = dr["KetThuc"].ToString() != "" ? int.Parse(dr["KetThuc"].ToString()) : 0;
            }
            catch
            {
                return null;
            }
        }
        return rs;
    }
    public List<TrangThaiDonHangOBJ> LayDanhSachTrangThaiDonHang(int idct)
    {
        List<TrangThaiDonHangOBJ> lst = new List<TrangThaiDonHangOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            DataTable dt = db.ExecuteDataSet("sp_App_LayDanhSachTrangThaiDonHang", param).Tables[0];

            foreach(DataRow dr in dt.Rows)
            {
                TrangThaiDonHangOBJ rs = new TrangThaiDonHangOBJ();
                rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
                rs.ID_TrangThaiDonHang = int.Parse(dr["ID_TrangThaiDonHang"].ToString());
                rs.TenTrangThai = dr["TenTrangThai"].ToString();
                rs.MauTrangThai = dr["MauTrangThai"].ToString();
                rs.TrangThaiXoa = dr["TrangThaiXoa"].ToString() != "" ? int.Parse(dr["TrangThaiXoa"].ToString()) : 0;
                rs.MacDinh = dr["MacDinh"].ToString() != "" ? int.Parse(dr["MacDinh"].ToString()) : 0;
                rs.KetThuc = dr["KetThuc"].ToString() != "" ? int.Parse(dr["KetThuc"].ToString()) : 0;
                rs.NgayXoa = dr["NgayXoa"].ToString() != "" ? DateTime.Parse(dr["NgayXoa"].ToString()) : rs.NgayXoa;
                rs.ID_QuanLyXoa = dr["ID_QuanLyXoa"].ToString() != "" ? int.Parse(dr["ID_QuanLyXoa"].ToString()) : 0;
                rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
                lst.Add(rs);
            }
            return lst;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return lst;
        }
    }

    public bool Them(TrangThaiDonHangOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                 
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenTrangThai", obj.TenTrangThai),
                    new SqlParameter("MauTrangThai", obj.MauTrangThai),
                    new SqlParameter("MacDinh", obj.MacDinh),
                    new SqlParameter("KetThuc", obj.KetThuc),
                };
            int i = db.ExecuteNonQuery("sp_TrangThaiDonHang_ThemMoi", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public bool Sua(TrangThaiDonHangOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_TrangThaiDonHang", obj.ID_TrangThaiDonHang),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenTrangThai", obj.TenTrangThai),
                    new SqlParameter("MauTrangThai", obj.MauTrangThai),
                    new SqlParameter("MacDinh", obj.MacDinh),
                     new SqlParameter("KetThuc", obj.KetThuc),
                    
                };
            int i = db.ExecuteNonQuery("sp_TrangThaiDonHang_Sua", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public bool Xoa(TrangThaiDonHangOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_TrangThaiDonHang", obj.ID_TrangThaiDonHang),
                    new SqlParameter("ID_QuanLyXoa", obj.ID_QuanLyXoa)
                    
                };
            int i = db.ExecuteNonQuery("sp_TrangThaiDonHang_Xoa", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
 
}