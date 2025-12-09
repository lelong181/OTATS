using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class DiaChiData
    {
        private SqlDataHelper helper;
        public DiaChiData()
        {
            helper = new SqlDataHelper();
        }
        public DataTable usp_vuongtm_getdanhsachtinh(int ID_QLLH)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_QLLH", ID_QLLH)
                };
                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_getdanhsachtinh", pars);
                dt = ds.Tables[0];
            }catch(Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public DataTable usp_vuongtm_getdanhsachquanhuyen()
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_getdanhsachquanhuyen");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public DataTable usp_vuongtm_getdanhsachxaphuong()
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_getdanhsachxaphuong");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
    }
}