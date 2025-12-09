using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LSPosMVC.Hubs;
using System.Data;

namespace LSPosMVC
{
    public sealed class DependencyTinNhan
    {
        public static readonly DependencyTinNhan instance = new DependencyTinNhan();
        private DependencyTinNhan()
        {
            conn = new SqlConnection(new SqlDataHelper().GetConnectionString());

        }
        public SqlDependency dependency { get; set; }
        public SqlConnection conn { get; set; }
        private string IdSqlDependence { get; set; }
        public void CreateCommand(SqlCommand cmd)
        {
            cmd.Notification = null;
            dependency = new SqlDependency(cmd);
            IdSqlDependence = dependency.Id;
            //dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // Process the DataReader.  
            }
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                if (((SqlDependency)sender).Id == IdSqlDependence)
                    TinNhanHub.SendMessages();
            }
        }
    }
}