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
    public class HotelsData
    {
        private SqlDataHelper helper;

        public HotelsData()
        {
            helper = new SqlDataHelper();
        }

        private HotelsModel GetHotelsModelFromDataRow(DataRow dr)
        {
            HotelsModel obj = new HotelsModel();
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (dr.Table.Columns.IndexOf(propertyInfo.Name) >= 0)
                {
                    if (dr[propertyInfo.Name] != DBNull.Value && !string.IsNullOrWhiteSpace(dr[propertyInfo.Name].ToString()))
                    {
                        Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                        var value = Convert.ChangeType(dr[propertyInfo.Name], t);
                        propertyInfo.SetValue(obj, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, null);
                    }
                }
            }

            return obj;
        }

        public List<HotelsModel> GetAll(int ID_QLLH)
        {
            List<HotelsModel> list = new List<HotelsModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_Hotels_GetAll", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetHotelsModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                throw ex;
            }
            return list;
        }

        public HotelsModel GetById(int HotelId)
        {
            HotelsModel obj = new HotelsModel();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", HotelId)
                };
                DataTable dt = helper.ExecuteDataSet("sp_Hotels_GetById", par).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    obj = GetHotelsModelFromDataRow(dt.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return obj;
        }

        public List<HotelsModel> GetFullStructure(int HotelId, int ID_QLLH)
        {
            List<HotelsModel> list = new List<HotelsModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", HotelId),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_Hotels_GetFullStructure", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetHotelsModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public int Insert(HotelsModel obj)
        {
            int NewId = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelName", obj.HotelName),
                    new SqlParameter("@Code", obj.Code),
                    new SqlParameter("@Address", obj.Address),
                    new SqlParameter("@Phone", obj.Phone),
                    new SqlParameter("@Email", obj.Email),
                    new SqlParameter("@Website", obj.Website),
                    new SqlParameter("@LogoUrl", obj.LogoUrl),
                    new SqlParameter("@ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                
                helper.ExecuteNonQuery("sp_Hotels_Insert", par);
                
                if (par[7].Value != DBNull.Value)
                {
                    NewId = Convert.ToInt32(par[7].Value);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                throw ex;
            }
            return NewId;
        }

        public bool Update(HotelsModel obj)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", obj.HotelId),
                    new SqlParameter("@HotelName", obj.HotelName),
                    new SqlParameter("@Code", obj.Code),
                    new SqlParameter("@Address", obj.Address),
                    new SqlParameter("@Phone", obj.Phone),
                    new SqlParameter("@Email", obj.Email),
                    new SqlParameter("@Website", obj.Website),
                    new SqlParameter("@LogoUrl", obj.LogoUrl),
                    new SqlParameter("@IsActive", obj.IsActive)
                };
                return helper.ExecuteNonQuery("sp_Hotels_Update", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
    }
}
