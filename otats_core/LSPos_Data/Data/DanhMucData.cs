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
    public class DanhMucData
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();

        public DanhMucData()
        {
            helper = new SqlDataHelper();
        }

        public DMTrangThaiDonHangModel GetTrangThaiFromDataRow(DataRow dr)
        {
            DMTrangThaiDonHangModel obj = new DMTrangThaiDonHangModel();
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (dr.Table.Columns.IndexOf(propertyInfo.Name) >= 0)
                {
                    if (!string.IsNullOrWhiteSpace(dr[propertyInfo.Name].ToString()))
                    {
                        var value = Convert.ChangeType(dr[propertyInfo.Name], propertyInfo.PropertyType);
                        propertyInfo.SetValue(obj, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, null);
                    }
                }
                else
                {
                    propertyInfo.SetValue(obj, null);
                }
            }
            return obj;
        }

        public bool checktrungmahaohut(string mahaohut, int idhaohut = 0)
        {
            int result = 0;
            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                    new SqlParameter("@mahaohut", mahaohut),
                    new SqlParameter("@idhaohut", idhaohut),
                };

                object id = helper.ExecuteScalar("loaihaohut_checktrungma", Parammeter);
                result = int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return (result > 0);
        }

        public DataTable getlisttrangthaidonhang(int ID_QLLH)
        {
            DataTable dt = new DataTable();

            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH)
                };

            try
            {
                DataSet ds = helper.ExecuteDataSet("trangthaidonhang_getlist", Parammeter);
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