using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class ChuongTrinhKMDAL
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();
        public ChuongTrinhKMDAL()
        {
            //
            // TODO: Add constructor logic here
            //
            helper = new SqlDataHelper();
        }

        public DataTable GetAllHinhThucKM(int ID_QuanLy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QuanLy", ID_QuanLy)
                };

                DataSet ds = helper.ExecuteDataSet("sp_HinhThucKhuyenMai_GetAll", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public DataTable GetDSKhuyenmaiCombo(int ID_QLLH)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllKhuyenMai_combo", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
        public DataSet GetDSKhuyenmaiAll(int ID_QLLH, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy)
                };

                ds = helper.ExecuteDataSet("sp_QL_GetAllKhuyenMai_v1", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }
        public DataSet GetDonHangByCTKM(int ID_CTKM)
        {
            SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_CTKM", ID_CTKM)
        };
            SqlDataHelper helper = new SqlDataHelper();
            DataSet ds = new DataSet();
            ds = helper.ExecuteDataSet("sp_QL_GetDonHang_ByID_CTKM", pars);


            try
            {
                return ds;
            }
            catch
            {
                return null;
            }
        }
        public DataSet GetChiTietKhuyenMai(int ID_QLLH, int ID_CTKM)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_CTKM", ID_CTKM)
        };
            SqlDataHelper helper = new SqlDataHelper();
            DataSet ds = new DataSet();
            ds = helper.ExecuteDataSet("sp_QL_GetChiTietKhuyenMai_ById", pars);

            try
            {
                return ds;
            }
            catch
            {
                return null;
            }
        }

    }
}