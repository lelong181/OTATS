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
    public class BaoCaoNhanVienViengThamData
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();

        public BaoCaoNhanVienViengThamData()
        {
            helper = new SqlDataHelper();
        }

        private BaoCaoNhanVienViengThamModel GetObjDataRow(DataRow dr)
        {
            BaoCaoNhanVienViengThamModel obj = new BaoCaoNhanVienViengThamModel();
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

        private BaoCaoSoLuongNhanVienViengThamModel GetObjSoLuongDataRow(DataRow dr)
        {
            BaoCaoSoLuongNhanVienViengThamModel obj = new BaoCaoSoLuongNhanVienViengThamModel();
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

        public List<BaoCaoNhanVienViengThamModel> GetDataBaoCao(int id_QLLH, int ID_Tuyen, int ID_NhanVien, DateTime tungay, DateTime denngay)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_QLLH", id_QLLH),
                new SqlParameter("@id_tuyen", ID_Tuyen),
                new SqlParameter("@from", tungay),
                new SqlParameter("@to", denngay),
                new SqlParameter("@idnhanvien", ID_NhanVien),
            };

            DataSet ds = new DataSet();
            List<BaoCaoNhanVienViengThamModel> databaocao = new List<BaoCaoNhanVienViengThamModel>();
            try
            {
                ds = helper.ExecuteDataSet("sp_BaoCaoCheckInTheoTuyen_Kendo", Param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        databaocao.Add(GetObjDataRow(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return databaocao;
        }

        public List<BaoCaoSoLuongNhanVienViengThamModel> GetDataBaoCaoSoLuong(int id_QLLH, int ID_Tuyen, int ID_NhanVien, DateTime tungay, DateTime denngay)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_QLLH", id_QLLH),
                new SqlParameter("@id_tuyen", ID_Tuyen),
                new SqlParameter("@from", tungay),
                new SqlParameter("@to", denngay),
                new SqlParameter("@idnhanvien", ID_NhanVien),
            };

            DataSet ds = new DataSet();
            List<BaoCaoSoLuongNhanVienViengThamModel> databaocao = new List<BaoCaoSoLuongNhanVienViengThamModel>();
            try
            {
                ds = helper.ExecuteDataSet("sp_BaoCaoViengThamKHTheoTuyen_Kendo", Param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        databaocao.Add(GetObjSoLuongDataRow(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return databaocao;
        }

    }
}