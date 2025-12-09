using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LSPosMVC.Hubs;
using System.Data;

namespace LSPosMVC
{
    public sealed class DependencyHoatDong
    {
        public static readonly DependencyHoatDong instance = new DependencyHoatDong();
        private DependencyHoatDong()
        {
            conn = new SqlConnection(new SqlDataHelper().GetConnectionString());

        }

        // Dependence hoạt động đăng nhập đăng xuất
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
                    HoatDongHub.SendMessages();
            }
        }


        // Dependence hoạt động checkin
        public SqlDependency dependency2 { get; set; }
        private string IdSqlDependence2 { get; set; }
        public void CreateCommand2(SqlCommand cmd)
        {
            cmd.Notification = null;
            dependency = new SqlDependency(cmd);
            IdSqlDependence = dependency.Id;
            //dependency.OnChange += new OnChangeEventHandler(dependency_OnChange2);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // Process the DataReader.  
            }
        }

        private void dependency_OnChange2(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                if (((SqlDependency)sender).Id == IdSqlDependence)
                    HoatDongHub.SendMessages();
            }
        }
    }
}