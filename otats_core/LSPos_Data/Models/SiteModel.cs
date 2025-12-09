using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class SiteModel
    {
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string ApiUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BankCode { get; set; }
        public string BankAccNumber { get; set; }
        public string WebPrintTicket { get; set; }
        public string SiteLogo { get; set; }
        public string SiteEmail { get; set; }
    }
}
