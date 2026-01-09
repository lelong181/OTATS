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
    public class DailyRatesData
    {
        private SqlDataHelper helper;

        public DailyRatesData()
        {
            helper = new SqlDataHelper();
        }

        private DailyRatesModel GetDailyRatesModelFromDataRow(DataRow dr)
        {
            DailyRatesModel obj = new DailyRatesModel();
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

        public List<DailyRatesModel> GetByDateRange(int RoomTypeId, int RatePlanId, DateTime FromDate, DateTime ToDate, int ID_QLLH)
        {
            List<DailyRatesModel> list = new List<DailyRatesModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomTypeId", RoomTypeId),
                    new SqlParameter("@RatePlanId", RatePlanId),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_DailyRates_GetByDateRange", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetDailyRatesModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public void InsertOrUpdate(DailyRatesModel obj)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomTypeId", obj.RoomTypeId),
                    new SqlParameter("@RatePlanId", obj.RatePlanId),
                    new SqlParameter("@Date", obj.Date),
                    new SqlParameter("@Price", obj.Price),
                    new SqlParameter("@Allotment", obj.Allotment),
                    new SqlParameter("@IsClosed", obj.IsClosed),
                    new SqlParameter("@IsClosed", obj.ID_QLLH)
                };
                helper.ExecuteNonQuery("sp_DailyRates_InsertOrUpdate", par);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
        }

        public void BulkUpdate(int RoomTypeId, int RatePlanId, DateTime FromDate, DateTime ToDate, decimal Price, bool IsClosed, int ID_QLLH)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RoomTypeId", RoomTypeId),
                    new SqlParameter("@RatePlanId", RatePlanId),
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@Price", Price),
                    new SqlParameter("@IsClosed", IsClosed),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                helper.ExecuteNonQuery("sp_DailyRates_BulkUpdate", par);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
        }
    }
}
