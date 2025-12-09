using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using LSPos_Data.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace LSPosMVC.Hubs
{
    public class DonHangHub : Hub
    {
        private static string conString = new SqlDataHelper().GetConnectionString();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<DonHangHub>();
        public void Hello()
        {
            Clients.All.hello();
        }

        [HubMethodName("sendMessages")]
        public static void SendMessages()
        {
            hubContext.Clients.All.updateMessages();
        }

        [HubMethodName("updatetrangthai")]
        public static void updatetrangthai()
        {
            hubContext.Clients.All.updatetrangthai();
        }


    }
}