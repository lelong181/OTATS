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
    public class PhanQuyen_KhachHangNhanVienData
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();

        public PhanQuyen_KhachHangNhanVienData()
        {
            helper = new SqlDataHelper();
        }

        private PhanQuyen_KhachHanngNhanVienModel GetObjDataRow(DataRow dr)
        {
            PhanQuyen_KhachHanngNhanVienModel obj = new PhanQuyen_KhachHanngNhanVienModel();
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

        public List<PhanQuyen_KhachHanngNhanVienModel> GetDSPhanQuyenNhanVien_TheoNhomKhachHang(int IDQLLH, int ID_Nhom, int ID_KhachHang)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Nhom", ID_Nhom),
            new SqlParameter("ID_KhachHang", ID_KhachHang)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSPhanQuyenNhanVien_TheoNhom_KhachHang", pars);
            DataTable dt = ds.Tables[0];
            try
            {
                List<PhanQuyen_KhachHanngNhanVienModel> dsnv = new List<PhanQuyen_KhachHanngNhanVienModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    PhanQuyen_KhachHanngNhanVienModel nv = GetObjDataRow(dr);
                    dsnv.Add(nv);
                }
                return dsnv;
            }
            catch
            {
                return null;
            }
        }

        public DataTable getnhanviendaphanchokhachhang(int IDQLLH, int ID_KhachHang)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_QLLH", IDQLLH),
                    new SqlParameter("ID_KhachHang", ID_KhachHang)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSPhanQuyenNhanVienByidKhachHang", pars);
                dt = ds.Tables[0];

            }
            catch(Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                
            }
            return dt;
        }
        public DataSet getdataexcelphanquyen(int ID_QLLH, int ID_QuanLy)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_QuanLy", ID_QuanLy),
                };

                ds = helper.ExecuteDataSet("sp_getkhachhangquyen_excel", pars);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return ds;
        }
    }
}