using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class ChuongTrinhKhuyenMaiData
    {
        private SqlDataHelper db;
        public ChuongTrinhKhuyenMaiData()
        {
            db = new SqlDataHelper();
        }
        public DataTable GetChiTietHangTang(int ID_CTKM)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_CTKM", ID_CTKM)
                };

                DataSet ds = db.ExecuteDataSet("sp_CTKM_GetChiTietHangTang", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public bool CheckDonHang(int ID_CTKM)
        {
            bool re = false;
            try
            {

                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_CTKM", ID_CTKM)
                };

                DataSet ds = db.ExecuteDataSet("VuongTM_KhuyenMai_CheckDonHang", pars);
                DataTable dt = ds.Tables[0];
                re = (Int32.Parse(dt.Rows[0]["re"].ToString()) > 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return re;
        }

    }
}