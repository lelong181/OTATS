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
    public class RatePlansData
    {
        private SqlDataHelper helper;

        public RatePlansData()
        {
            helper = new SqlDataHelper();
        }

        private RatePlansModel GetRatePlansModelFromDataRow(DataRow dr)
        {
            RatePlansModel obj = new RatePlansModel();
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

        public List<RatePlansModel> GetByHotel(int HotelId, int ID_QLLH, bool IncludeInactive = false)
        {
            List<RatePlansModel> list = new List<RatePlansModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", HotelId),
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@IncludeInactive", IncludeInactive)
                };
                DataTable dt = helper.ExecuteDataSet("sp_RatePlans_GetByHotel", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetRatePlansModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public int Insert(RatePlansModel obj)
        {
            int NewId = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@HotelId", obj.HotelId),
                    new SqlParameter("@PlanCode", obj.PlanCode),
                    new SqlParameter("@PlanName", obj.PlanName),
                    new SqlParameter("@IsBreakfastIncluded", obj.IsBreakfastIncluded ?? (object)DBNull.Value),
                    new SqlParameter("@IsLunchIncluded", obj.IsLunchIncluded ?? (object)DBNull.Value),
                    new SqlParameter("@IsDinnerIncluded", obj.IsDinnerIncluded ?? (object)DBNull.Value),
                    new SqlParameter("@IsRefundable", obj.IsRefundable ?? (object)DBNull.Value),
                    new SqlParameter("@MinLengthOfStay", obj.MinLengthOfStay ?? (object)DBNull.Value),
                    new SqlParameter("@MaxLengthOfStay", obj.MaxLengthOfStay ?? (object)DBNull.Value),
                    new SqlParameter("@ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                
                helper.ExecuteNonQuery("sp_RatePlans_Insert", par);
                
                if (par[9].Value != DBNull.Value)
                {
                    NewId = Convert.ToInt32(par[9].Value);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                // Can rethrow to let Controller handle constraint errors like Duplicate PlanCode
                throw ex;
            }
            return NewId;
        }

        public bool Update(RatePlansModel obj)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@RatePlanId", obj.RatePlanId),
                    new SqlParameter("@PlanName", obj.PlanName),
                    new SqlParameter("@IsBreakfastIncluded", obj.IsBreakfastIncluded ?? (object)DBNull.Value),
                    new SqlParameter("@IsLunchIncluded", obj.IsLunchIncluded ?? (object)DBNull.Value),
                    new SqlParameter("@IsDinnerIncluded", obj.IsDinnerIncluded ?? (object)DBNull.Value),
                    new SqlParameter("@IsRefundable", obj.IsRefundable ?? (object)DBNull.Value),
                    new SqlParameter("@MinLengthOfStay", obj.MinLengthOfStay ?? (object)DBNull.Value),
                    new SqlParameter("@MaxLengthOfStay", obj.MaxLengthOfStay ?? (object)DBNull.Value),
                    new SqlParameter("@IsActive", obj.IsActive ?? (object)DBNull.Value)
                };
                return helper.ExecuteNonQuery("sp_RatePlans_Update", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
    }
}
