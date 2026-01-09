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
    public class RoomTypesData
    {
        private SqlDataHelper helper;

        public RoomTypesData()
        {
            helper = new SqlDataHelper();
        }

        private RoomTypesModel GetRoomTypesModelFromDataRow(DataRow dr)
        {
            RoomTypesModel obj = new RoomTypesModel();
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

        public List<RoomTypesModel> GetByHotel(int HotelId, int ID_QLLH)
        {
            List<RoomTypesModel> list = new List<RoomTypesModel>();
            try
            {
                 SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", HotelId),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_RoomTypes_GetByHotel", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetRoomTypesModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public int Insert(RoomTypesModel obj)
        {
            int NewId = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", obj.HotelId),
                    new SqlParameter("@TypeName", obj.TypeName),
                    new SqlParameter("@BasePrice", obj.BasePrice),
                    new SqlParameter("@MaxAdults", obj.MaxAdults),
                    new SqlParameter("@MaxChildren", obj.MaxChildren),
                    new SqlParameter("@AreaSquareMeter", obj.AreaSquareMeter),
                    new SqlParameter("@Description", obj.Description),
                    new SqlParameter("@ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                
                helper.ExecuteNonQuery("sp_RoomTypes_Insert", par);
                
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

        public bool Update(RoomTypesModel obj)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomTypeId", obj.RoomTypeId),
                    new SqlParameter("@TypeName", obj.TypeName),
                    new SqlParameter("@BasePrice", obj.BasePrice),
                    new SqlParameter("@MaxAdults", obj.MaxAdults),
                    new SqlParameter("@MaxChildren", obj.MaxChildren),
                    new SqlParameter("@AreaSquareMeter", obj.AreaSquareMeter),
                    new SqlParameter("@Description", obj.Description)
                };
                return helper.ExecuteNonQuery("sp_RoomTypes_Update", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
    }
}
