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
    public class ZonesData
    {
        private SqlDataHelper helper;

        public ZonesData()
        {
            helper = new SqlDataHelper();
        }

        private ZonesModel GetZonesModelFromDataRow(DataRow dr)
        {
            ZonesModel obj = new ZonesModel();
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

        public List<ZonesModel> GetByHotel(int HotelId, int ID_QLLH)
        {
            List<ZonesModel> list = new List<ZonesModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", HotelId),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_Zones_GetByHotel", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetZonesModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                throw ex; // Re-throw to controller
            }
            return list;
        }

        public int Insert(ZonesModel obj)
        {
            int NewId = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", obj.HotelId),
                    new SqlParameter("@ZoneName", obj.ZoneName),
                    new SqlParameter("@Description", obj.Description),
                    new SqlParameter("@DistanceToReception", obj.DistanceToReception),
                    new SqlParameter("@ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                
                helper.ExecuteNonQuery("sp_Zones_Insert", par);
                
                if (par[4].Value != DBNull.Value)
                {
                    NewId = Convert.ToInt32(par[4].Value);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                throw ex;
            }
            return NewId;
        }

        public bool Update(ZonesModel obj)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@ZoneId", obj.ZoneId),
                    new SqlParameter("@ZoneName", obj.ZoneName),
                    new SqlParameter("@Description", obj.Description),
                    new SqlParameter("@DistanceToReception", obj.DistanceToReception)
                };
                return helper.ExecuteNonQuery("sp_Zones_Update", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                throw ex;
            }
        }

        public bool Delete(int ZoneId)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@ZoneId", ZoneId)
                };
                return helper.ExecuteNonQuery("sp_Zones_Delete", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                throw ex;
            }
        }
    }
}
