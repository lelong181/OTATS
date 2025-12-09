using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class ChiTieuKPIData
    {
        private SqlDataHelper helper;
        public ChiTieuKPIData()
        {
            helper = new SqlDataHelper();
        }

        public bool Delete(string ListID_ChiTieuKPI)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ListID_ChiTieuKPI",ListID_ChiTieuKPI),
                };

                if (helper.ExecuteNonQuery("sp_ChiTieuKPI_Delete_Multi", pars) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public DataTable GetChiTieuTheoNhanVienKPI(int ID_QLLH, int ID_Nhom,int ID_QuanLy)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                 new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_Nhom", ID_Nhom)
                };

                DataSet ds = helper.ExecuteDataSet("sp_ChiTieuKPI_GetDanhSach_v2", pars);
                dt = ds.Tables[0];





            }
            catch (Exception ex)
            {
                return null;
            }

            return dt;
        }
    }
}