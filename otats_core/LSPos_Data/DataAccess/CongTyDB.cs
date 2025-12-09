using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class CongTyDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CongTyDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public CongTyDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool KiemTraHanMucTaiKhoan(int idct)
    {
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@idct", idct),

            };
            if (int.Parse(db.ExecuteScalar("sp_App_KiemTraHanMuc", param).ToString()) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
         catch(Exception ex)
        {
            log.Error(ex);
        }
        return false;
    }
     
    public static CongTyOBJ ThongTinCongTyByID(int idct)
    {
        CongTyOBJ rs = new CongTyOBJ();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@id", idct)
            };
            DataRow dr = db.ExecuteDataSet("sp_App_GetThongTinCongTyTheoID", param).Tables[0].Rows[0];
            rs.idcongty = int.Parse(dr["ID_QLLH"].ToString());
            rs.tencongty = dr["MoTa"].ToString();
            rs.sodienthoai = dr["SDT"].ToString();
            rs.thoigiancapnhatbantin = int.Parse(dr["ThoiGianCapNhatBanTin"].ToString());
            rs.goiungdungid = dr["idgoiungdung"].ToString() != "" ?  int.Parse(dr["idgoiungdung"].ToString()) : 0;
            rs.urlserver = dr["urlserver"].ToString();
            rs.bankinhchophep = dr["bankinhchophep"].ToString() != "" ? double.Parse(dr["bankinhchophep"].ToString()) : 500;
            rs.thoihanhopdong = dr["ThoiHanHopDong"].ToString() != "" ? DateTime.Parse(dr["ThoiHanHopDong"].ToString()) : new DateTime(1900,01,01);
            rs.soluongnhanvien_duocap = dr["soluongnhanvien_duocap"].ToString() != "" ? int.Parse(dr["soluongnhanvien_duocap"].ToString()) : 0;
            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }

    public static int TimKiemIDCT(string input)
    {

        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@input", input)
            };
            return int.Parse(db.ExecuteScalar("sp_App_TimIDCT", param).ToString());
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return 0;
        }
    }

    public List<CongTyOBJ> GetDSCongTy()
    {
        DataSet ds = db.ExecuteDataSet("sp_GetAllCongTy");
        DataTable dt = ds.Tables[0];
        List<CongTyOBJ> rs = new List<CongTyOBJ>();
        if (dt.Rows.Count == 0)
            return null;
        try
        {
            foreach (DataRow dr in dt.Rows)
            {
                CongTyOBJ dsct = new CongTyOBJ();
                dsct.idcongty = int.Parse(dr["ID_QLLH"].ToString());
                dsct.tencongty = dr["MoTa"].ToString();
                rs.Add(dsct);
            }
            return rs;
        }
        catch
        {
            return null;
        }
    }
    public DataTable GetTinNhan(int ID_QLLH)
    {
        try 
        {
            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH",ID_QLLH)
            };

            DataSet ds = db.ExecuteDataSet("sp_QL_GetTinNhan_ID_QLLH", par);
            DataTable dt = ds.Tables[0];
            return dt;
        }
        catch (Exception ex) 
        {
            return null;
        }
    }

    public static string GetConfigValue(int ID_QLLH, string ParamName)
    {
        string _value = "";
        SqlParameter[] par = new SqlParameter[]{
            new SqlParameter("@ID_QLLH",ID_QLLH),
            new SqlParameter("@ParamName",ParamName)

        };

        SqlDataHelper helper = new SqlDataHelper();
        object objData = helper.ExecuteScalar("sp_Config_GetByParamName", par);
        if (objData != null)
        {
            _value = objData.ToString();
        }
        return _value;
    }
}