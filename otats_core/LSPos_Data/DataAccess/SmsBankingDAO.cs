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
public class SmsBankingDAO
{ 
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SmsBankingDAO));
    public static SqlDataHelper db = new SqlDataHelper();
    public SmsBankingDAO()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<SmsBankingModel> GetAllByDate(DateTime date)
    {
        List<SmsBankingModel> rs = new List<SmsBankingModel>();

        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@date", date),
            };
            DataTable dt = db.ExecuteDataSet("sp_SmsBanking_GetAllByDate").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                SmsBankingModel item = GetObjectFromDataRowUtil<SmsBankingModel>.ToOject(dr);
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

    public bool Insert(SmsBankingModel obj)
    {
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@Amount", obj.Amount),
                new SqlParameter("@Content", obj.Content),
                new SqlParameter("@ID_DonHang", obj.ID_DonHang),
                new SqlParameter("@PhoneNumber", obj.PhoneNumber),
                new SqlParameter("@RecieveDate", obj.RecieveDate)
            };
            int rowwaff = db.ExecuteNonQuery("sp_SmsBanking_Insert", par);
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