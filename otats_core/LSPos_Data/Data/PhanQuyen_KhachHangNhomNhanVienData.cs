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
    public class PhanQuyen_KhachHangNhomNhanVienData
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();

        public PhanQuyen_KhachHangNhomNhanVienData()
        {
            helper = new SqlDataHelper();
        }

        private PhanQuyen_KhachHangNhomNhanVienModel GetObjDataRow(DataRow dr)
        {
            PhanQuyen_KhachHangNhomNhanVienModel obj = new PhanQuyen_KhachHangNhomNhanVienModel();
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

        public List<PhanQuyen_KhachHangNhomNhanVienModel> GetDSPhanQuyenNhanVien_TheoNhomKhachHang(int IDQLLH, int ID_KhachHang)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_QLLH", IDQLLH),
                    new SqlParameter("ID_KhachHang", ID_KhachHang)
                };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSPhanQuyenNhomNhanVien_KhachHang", pars);
                DataTable dt = ds.Tables[0];

                List<PhanQuyen_KhachHangNhomNhanVienModel> dsnv = new List<PhanQuyen_KhachHangNhomNhanVienModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    PhanQuyen_KhachHangNhomNhanVienModel nv = GetObjDataRow(dr);
                    dsnv.Add(nv);
                }
                return dsnv;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
    }
}