using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class PhieuDieuChuyenData
    {
        private SqlDataHelper helper;
        public PhieuDieuChuyenData()
        {
            helper = new SqlDataHelper();
        }

        public DataSet getexcelphieudieuchuyen(int ID_QLLH, DateTime from, DateTime to)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to)
                };

                ds = helper.ExecuteDataSet("usp_vuongtm_phieudieuchuyen_getdanhsachexcel", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return ds;
        }
    }
}