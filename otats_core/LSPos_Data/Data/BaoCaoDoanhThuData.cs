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
    public class BaoCaoDoanhThuData
    {
        private SqlDataHelper helper;
        public BaoCaoDoanhThuData()
        {
            helper = new SqlDataHelper();
        }

        public BaoCaoDoanhThuModel DataFromDataRow(DataRow dr)
        {
            BaoCaoDoanhThuModel obj = new BaoCaoDoanhThuModel();
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


        public List<BaoCaoDoanhThuModel> GetBaoCaoDoanhThu_NhanVien(int ID_QLLH, int ID_QuanLy, string FromMonth, string FromYear, string ToMonth, string ToYear, int ID_NhanVien, int ID_Nhom, int ID_KhachHang)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("frommounth", FromMonth),
                new SqlParameter("fromyear", FromYear),
                new SqlParameter("tomounth", ToMonth),
                new SqlParameter("toyear", ToYear),
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("ID_Nhom", ID_Nhom),
                new SqlParameter("ID_KhachHang", ID_KhachHang)
            };

            DataSet ds = helper.ExecuteDataSet("usp_vuongtm_baocaodoanhthutheothang", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;

            try
            {
                List<BaoCaoDoanhThuModel> dsdv = new List<BaoCaoDoanhThuModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    BaoCaoDoanhThuModel dv = new BaoCaoDoanhThuModel();
                    dv = DataFromDataRow(dr);
                    dsdv.Add(dv);
                }

                return dsdv;
            }
            catch
            {
                return null;
            }
        }

    }
}