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
    public class FloorsData
    {
        private SqlDataHelper helper;

        public FloorsData()
        {
            helper = new SqlDataHelper();
        }

        private FloorsModel GetFloorsModelFromDataRow(DataRow dr)
        {
            FloorsModel obj = new FloorsModel();
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

        public List<FloorsModel> GetByBuilding(int BuildingId, int ID_QLLH)
        {
            List<FloorsModel> list = new List<FloorsModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@BuildingId", BuildingId),
                    new SqlParameter("@ID_QLLH", ID_QLLH)
                };
                DataTable dt = helper.ExecuteDataSet("sp_Floors_GetByBuilding", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetFloorsModelFromDataRow(dr));
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return list;
        }

        public int Insert(FloorsModel obj)
        {
            int NewId = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[] {
                    new SqlParameter("@BuildingId", obj.BuildingId),
                    new SqlParameter("@FloorNumber", obj.FloorNumber),
                    new SqlParameter("@FloorName", obj.FloorName),
                    new SqlParameter("@MapImage", obj.MapImage),
                    new SqlParameter("@ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                
                helper.ExecuteNonQuery("sp_Floors_Insert", par);
                
                if (par[4].Value != DBNull.Value)
                {
                    NewId = Convert.ToInt32(par[4].Value);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return NewId;
        }
    }
}
