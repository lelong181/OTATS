using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class PhanQuyenData
    {
        private SqlDataHelper helper;
        public PhanQuyenData()
        {
            helper = new SqlDataHelper();
        }

        public bool ThemChucNangChoNhom(int ID_Nhom, int ID_ChucNang)
        {
            bool flag = true;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_Nhom", ID_Nhom),
                    new SqlParameter("@ID_ChucNang",ID_ChucNang)
                    };

                if (helper.ExecuteNonQuery("sp_Insert_PQChucNang_Nhom", pars) <= 0)
                    flag = false;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return flag;
        }

        public bool ThemChucNangChoNhom_Quyen(int ID_Nhom, int ID_ChucNang, int Them, int Sua, int Xoa)
        {
            bool flag = true;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_Nhom", ID_Nhom),
                    new SqlParameter("@ID_ChucNang",ID_ChucNang),
                    new SqlParameter("@Them",Them),
                    new SqlParameter("@Sua",Sua),
                    new SqlParameter("@Xoa",Xoa)
                    };

                if (helper.ExecuteNonQuery("sp_Insert_PQChucNang_Nhom_v2", pars) <= 0)
                    flag = false;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return flag;
        }

        public bool DeleteChucNang_Nhom(int ID_Nhom)
        {
            bool flag = true;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_Nhom", ID_Nhom)
                    };

                if (helper.ExecuteNonQuery("sp_Delete_PQChucNang_Nhom", pars) <= 0)
                    flag = false;
            }
            catch(Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return flag;
        }

        public DataTable chucnangapp_getlist(int ID_QLLH, int ID_Nhom)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_Nhom", ID_Nhom)
                };

                DataSet ds = helper.ExecuteDataSet("phanquyen_chucnangapp_getlist", Parammeter);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public DataTable chucnangweb_getlist(int ID_QLLH, int ID_Nhom)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_Nhom", ID_Nhom)
                };

                DataSet ds = helper.ExecuteDataSet("phanquyen_chucnangweb_getlist", Parammeter);
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