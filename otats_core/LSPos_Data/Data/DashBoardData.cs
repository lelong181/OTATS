using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LSPos_Data.Data
{
    public class DashBoardData
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();

        public DashBoardData()
        {
            helper = new SqlDataHelper();
        }

        public DataTable getdatabox(int ID_QLLH, int ID_QuanLy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] Param = new SqlParameter[]
                {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy)
                };
                DataSet ds = helper.ExecuteDataSet("Dashboard_vuongtm_getdatabox", Param);
                dt = ds.Tables[0];
            }
            catch(Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return dt;
        }

        public DataSet getdatachart(int ID_QLLH, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[]
                {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy)
                };
                ds = helper.ExecuteDataSet("Dashboard_vuongtm_getdatachart", Param);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return ds;
        }
    }
}