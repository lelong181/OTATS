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
public class GlobalDAO
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(GlobalDAO));
    public static SqlDataHelper db = new SqlDataHelper();
    public GlobalDAO()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public GlobalModel GetCurrentGlobalBySite(string sitecode, DateTime date)
    {

        SqlParameter[] param = new SqlParameter[]
          {
                new SqlParameter("@SiteCode", sitecode),
                new SqlParameter("@SessionDate", date),

          };
        try
        {
            DataTable dt = db.ExecuteDataSet("sp_Global_GetCurrentBySite", param).Tables[0];
            DataRow dr = dt.Rows[0];
            GlobalModel item = GetObjectFromDataRowUtil<GlobalModel>.ToOject(dr);
            return item;

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public bool InsertGlobal(GlobalModel obj)
    {
        int ID = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@SiteID", obj.SiteID),
                new SqlParameter("@SiteCode", obj.SiteCode),
                new SqlParameter("@ZoneID", obj.ZoneID),
                new SqlParameter("@ComputerID", obj.ComputerID),
                new SqlParameter("@Fullname", obj.Fullname),
                new SqlParameter("@Username", obj.Username),
                new SqlParameter("@ConnectKey", obj.ConnectKey),
                new SqlParameter("@ConnectID", obj.ConnectID),
                new SqlParameter("@SessionID", obj.SessionID),
                 new SqlParameter("@SessionNo", obj.SessionNo)
            };
            int rowwaff = db.ExecuteNonQuery("sp_Global_Insert", par);
            if (rowwaff > 0)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;

        }
        return false;

    }
}