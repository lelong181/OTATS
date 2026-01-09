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
    public class RoomsData
    {
        private SqlDataHelper helper;

        public RoomsData()
        {
            helper = new SqlDataHelper();
        }

        private RoomsModel GetRoomsModelFromDataRow(DataRow dr)
        {
            RoomsModel obj = new RoomsModel();
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

        public List<RoomsModel> GetAll(int? HotelId, int? BuildingId, int? FloorId, string Status, int ID_QLLH)
        {
            List<RoomsModel> list = new List<RoomsModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", HotelId),
                    new SqlParameter("@BuildingId", BuildingId),    
                    new SqlParameter("@FloorId", FloorId),
                    new SqlParameter("@Status", Status),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_Rooms_GetAll", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetRoomsModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public long Insert(RoomsModel obj)
        {
            long NewId = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@FloorId", obj.FloorId),
                    new SqlParameter("@RoomNumber", obj.RoomNumber),
                    new SqlParameter("@RoomName", obj.RoomName),
                    new SqlParameter("@RoomTypeId", obj.RoomTypeId),
                    new SqlParameter("@IsSmokingAllowed", obj.IsSmokingAllowed),
                    new SqlParameter("@IsConnectingRoom", obj.IsConnectingRoom),
                    new SqlParameter("@ConnectedRoomId", obj.ConnectedRoomId),
                    new SqlParameter("@ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
                };
                
                helper.ExecuteNonQuery("sp_Rooms_Insert", par);
                
                if (par[7].Value != DBNull.Value)
                {
                    NewId = Convert.ToInt64(par[7].Value);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return NewId;
        }

        public bool UpdateStatus(long RoomId, string Status, string CleanStatus)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomId", RoomId),
                    new SqlParameter("@Status", Status),
                    new SqlParameter("@CleanStatus", CleanStatus)
                };
                return helper.ExecuteNonQuery("sp_Rooms_UpdateStatus", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
    }
}
