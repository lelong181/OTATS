using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Data
{
    public class GuiBanMatHangData
    {
        private SqlDataHelper helper;
        public GuiBanMatHangData()
        {
            helper = new SqlDataHelper();
        }

        private KhachHang_HangGuiModel GetObjDataRow(DataRow dr)
        {
            KhachHang_HangGuiModel obj = new KhachHang_HangGuiModel();
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

        public List<KhachHang_HangGuiModel> GetAllByKhachHang(int ID_KhachHang, int ID_QLLH)
        {
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_KhachHang", ID_KhachHang)
            };

            List<KhachHang_HangGuiModel> result = new List<KhachHang_HangGuiModel>();

            try
            {
                DataTable dt = helper.ExecuteDataSet("GuiBanMatHang_GetByKhachHang", Parammeter).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    KhachHang_HangGuiModel item = new KhachHang_HangGuiModel();

                    item.ID = int.Parse(dr["ID"].ToString());
                    item.ID_KhachHang = int.Parse(dr["ID_KhachHang"].ToString());
                    item.ID_MatHang = int.Parse(dr["ID_MatHang"].ToString());
                     
                    item.MaHang = dr["MaHang"].ToString();
                    item.TenMatHang = dr["TenHang"].ToString();
                    item.TenDonVi = dr["TenDonVi"].ToString();
                    item.DienGiai = dr["DienGiai"].ToString();

                    try
                    {
                        item.NgayBatDau = Convert.ToDateTime(dr["NgayBatDau"].ToString());
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }

                    try
                    {
                        item.NgayKetThuc = Convert.ToDateTime(dr["NgayKetThuc"].ToString());
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }

                    result.Add(item);
                }
                
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return result;
        }

        public List<KhachHang_HangGuiModel> GetAllKhachHangGui(int ID_QLLH)
        {
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH)
            };

            List<KhachHang_HangGuiModel> result = new List<KhachHang_HangGuiModel>();

            try
            {
                DataTable dt = helper.ExecuteDataSet("GuiBanMatHang_AllKhachHangGui", Parammeter).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    KhachHang_HangGuiModel item = GetObjDataRow(dr);
                    result.Add(item);
                }
                return result;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return result;
            }
        }

        public List<MatHang> getMatHangbykhachhang(int id_QLLH, int idKhachHang)
        {
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@id_QLLH", id_QLLH),
                new SqlParameter("@idKhachHang", idKhachHang)
            };

            List<MatHang> result = new List<MatHang>();

            try
            {
                DataTable dt = helper.ExecuteDataSet("usp_vuongtm_GuiBanMatHang_getmathang", Parammeter).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    MatHang item = new MatHang();
                    item.IDMatHang = int.Parse(dr["ID_Hang"].ToString());
                    item.TenHang = dr["TenHang"].ToString();
                    item.MaHang = dr["MaHang"].ToString();
                    item.TenDonVi = dr["TenDonVi"].ToString();

                    result.Add(item);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return result;
        }

        public int add(KhachHang_HangGuiModel guiBanMatHang)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID", guiBanMatHang.ID),
                    new SqlParameter("@ID_QLLH", guiBanMatHang.ID_QLLH),
                    new SqlParameter("@ID_KhachHang", guiBanMatHang.ID_KhachHang),
                    new SqlParameter("@ID_MatHang", guiBanMatHang.ID_MatHang),
                    new SqlParameter("@NgayBatDau", guiBanMatHang.NgayBatDau),
                    new SqlParameter("@NgayKetThuc", guiBanMatHang.NgayKetThuc),
                    new SqlParameter("@DienGiai", guiBanMatHang.DienGiai),
                    new SqlParameter("@InsertedBy", guiBanMatHang.UpdatedBy),
                };

                object obj = helper.ExecuteScalar("usp_vuongtm_GuiBanMatHang_add", param);
                if (obj != null)
                {
                    int x = int.Parse(obj.ToString());
                    if (x > 1)
                    {
                        result = x;
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return result;
        }

        public bool deletemarkmulti(int idKhachHang, string Listid, string UpdatedBy)
        {
            bool result = false;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@idKhachHang", idKhachHang),
                    new SqlParameter("@Listid", Listid),
                    new SqlParameter("@UpdatedBy", UpdatedBy)
                };

                result = helper.ExecuteNonQuery("usp_vuongtm_GuiBanMatHang_DeleteMarkmulti", param) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return result;
        }

        public bool Create(KhachHang_HangGuiModel item)
        {
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", item.ID_QLLH),
                new SqlParameter("@ID_KhachHang", item.ID_KhachHang),
                new SqlParameter("@ID_MatHang", item.ID_MatHang)

            };

            int result = 0;

            try
            {
                result = helper.ExecuteNonQuery("GuiBanMatHang_Insert", Parammeter);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }

        }

        public bool Delete(int ID)
        {
            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_GuiBanMatHang", ID),

            };

            int result = 0;

            try
            {
                result = helper.ExecuteNonQuery("GuiBanMatHang_DeleteByID", Parammeter);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }

        }
    }
}