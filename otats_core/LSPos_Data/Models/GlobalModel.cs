using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class GlobalModel
    {
        public string SiteCode { get; set; }
        public string SiteID { get; set; }
        public string ZoneID { get; set; }
        public string Fullname { get; set; }
        public string ComputerID { get; set; }
        public string Username { get; set; }
        public string ConnectKey { get; set; }
        public string SessionID { get; set; }
        public string SessionNo { get; set; }
        public DateTime SessionDate { get; set; }
        public string Status { get; set; }
        public string ConnectID { get; set; }
    }
}
