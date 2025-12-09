using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPosMVC.Hubs
{
    public class NotiDonHangHub : Hub
    {
        [HubMethodName("Send")]
        public void Send()
        {
            Clients.All.callMessage();
        }
    }
}