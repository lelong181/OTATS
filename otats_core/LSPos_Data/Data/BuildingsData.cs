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
    public class BuildingsData
    {
        private SqlDataHelper helper;

        public BuildingsData()
        {
            helper = new SqlDataHelper();
        }

        private BuildingsModel GetBuildingsModelFromDataRow(DataRow dr)
        {
            BuildingsModel obj = new BuildingsModel();
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

        public List<BuildingsModel> GetByZone(int ZoneId, int ID_QLLH)
        {
            List<BuildingsModel> list = new List<BuildingsModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@ZoneId", ZoneId),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_Buildings_GetByZone", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetBuildingsModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public int Insert(BuildingsModel obj)
        {
            int NewId = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@ZoneId", obj.ZoneId),
                    new SqlParameter("@BuildingName", obj.BuildingName),
                    new SqlParameter("@TotalFloors", obj.TotalFloors ?? (object)DBNull.Value),
                    new SqlParameter("@HasElevator", obj.HasElevator ?? (object)DBNull.Value),
                    new SqlParameter("@ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                
                helper.ExecuteNonQuery("sp_Buildings_Insert", par);
                
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

        public bool Update(BuildingsModel obj)
        {
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@BuildingId", obj.BuildingId),
                    new SqlParameter("@BuildingName", obj.BuildingName),
                    new SqlParameter("@TotalFloors", obj.TotalFloors ?? (object)DBNull.Value),
                    new SqlParameter("@HasElevator", obj.HasElevator ?? (object)DBNull.Value)
                };
                return helper.ExecuteNonQuery("sp_Buildings_Update", par) > 0;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }
    }
}
