using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LSPosMVC.Hubs;
using System.Data;

namespace LSPosMVC
{    
    public sealed class DependencyDonhang
    {
        public static readonly DependencyDonhang instance = new DependencyDonhang();
        private DependencyDonhang()
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
            
            dependency.OnChange += new OnChangeEventHandler(dependency_OnChangeDonHang);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // Process the DataReader.  
            }
        }

        private void dependency_OnChangeDonHang(object sender, SqlNotificationEventArgs e)
        {
            
            if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Insert)
            {
                if (((SqlDependency)sender).Id == IdSqlDependence)
                {
                    DonHangHub.SendMessages();
                }
            }
            else if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Update)
            {
                if (((SqlDependency)sender).Id == IdSqlDependence)
                    DonHangHub.updatetrangthai();
            }
        }
    }
}