using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

/// <summary>
/// Summary description for KeHoachBaoDuongDB
/// </summary>
public class DichVuDAO
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(DichVuDAO));
    public static SqlDataHelper db = new SqlDataHelper();
    public DichVuDAO()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<DichVuModel> GetAllDichVu()
    {
        List<DichVuModel> rs = new List<DichVuModel>();

        try
        {
            DataTable dt = db.ExecuteDataSet("sp_DichVu_GetAll").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                DichVuModel item = GetObjectFromDataRowUtil<DichVuModel>.ToOject(dr);
                rs.Add(item);
            }
            return rs;

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }

    public DichVuModel GetByID(int ID)
    {
        SqlParameter[] param = new SqlParameter[]
       {
                new SqlParameter("@ID", ID),

       };
        try
        {
            DataTable dt = db.ExecuteDataSet("sp_DichVu_GetByID", param).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DichVuModel item = GetObjectFromDataRowUtil<DichVuModel>.ToOject(dt.Rows[0]);
                return item;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public bool CapNhatNCCDichVu(DichVuModel mh)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID", mh.ID),
                new SqlParameter("ID_NhaCungCap", mh.ID_NhaCungCap)
            };

            if (db.ExecuteNonQuery("sp_DichVu_UpdateNCC", pars) != 0)
            {
                // cap nhat thanh cong
                return true;
            }
            else
            {
                // cap nhat that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }

    public List<DichVuModel> GetBySiteCode(string SiteCode)
    {
        List<DichVuModel> rs = new List<DichVuModel>();
        SqlParameter[] param = new SqlParameter[]
       {
                new SqlParameter("@SiteCode", SiteCode),

       };
        try
        {
            DataTable dt = db.ExecuteDataSet("sp_DichVu_GetBySiteCode", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                DichVuModel item = GetObjectFromDataRowUtil<DichVuModel>.ToOject(dr);
                rs.Add(item);
            }
            return rs;

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }
}