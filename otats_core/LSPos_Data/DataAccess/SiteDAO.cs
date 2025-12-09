using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachBaoDuongDB
/// </summary>
public class SiteDAO
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SiteDAO));
    public static SqlDataHelper db = new SqlDataHelper();
    public SiteDAO()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<SiteModel> GetAllSite()
    {
        List<SiteModel> rs = new List<SiteModel>();

        try
        {
            DataTable dt = db.ExecuteDataSet("sp_Site_GetAll").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                SiteModel item = GetObjectFromDataRowUtil<SiteModel>.ToOject(dr);
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

    public SiteModel GetSite(string sitecode)
    {

        SqlParameter[] param = new SqlParameter[]
          {
                new SqlParameter("@SiteCode", sitecode),

          };
        try
        {
            DataTable dt = db.ExecuteDataSet("sp_Site_GetBySiteCode", param).Tables[0];
            DataRow dr = dt.Rows[0];
            SiteModel item = GetObjectFromDataRowUtil<SiteModel>.ToOject(dr);
            return item;

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
}