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
    public class RoomTypeRatePlansData
    {
        private SqlDataHelper helper;

        public RoomTypeRatePlansData()
        {
            helper = new SqlDataHelper();
        }

        private RoomTypeRatePlansModel GetRoomTypeRatePlansModelFromDataRow(DataRow dr)
        {
            RoomTypeRatePlansModel obj = new RoomTypeRatePlansModel();
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
            }
            return obj;
        }

        public List<RoomTypeRatePlansModel> GetByRoomType(int RoomTypeId, int ID_QLLH)
        {
            List<RoomTypeRatePlansModel> list = new List<RoomTypeRatePlansModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomTypeId", RoomTypeId),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_RoomTypeRatePlans_GetByRoomType", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetRoomTypeRatePlansModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public void InsertOrUpdate(RoomTypeRatePlansModel obj)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomTypeId", obj.RoomTypeId),
                    new SqlParameter("@RatePlanId", obj.RatePlanId),
                    new SqlParameter("@DefaultPrice", obj.DefaultPrice),
                    new SqlParameter("@IsActive", obj.IsMappingActive ?? (object)DBNull.Value)
                };
                
                helper.ExecuteNonQuery("sp_RoomTypeRatePlans_InsertOrUpdate", par);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                throw ex;
            }
        }

        public bool Delete(int RoomTypeId, int RatePlanId)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomTypeId", RoomTypeId),
                    new SqlParameter("@RatePlanId", RatePlanId)
                };
                return helper.ExecuteNonQuery("sp_RoomTypeRatePlans_Delete", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
    }
}
