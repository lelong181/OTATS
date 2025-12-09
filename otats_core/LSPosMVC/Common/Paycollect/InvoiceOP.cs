using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common.Paycollect
{
    public class InvoiceOP
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string state { get; set; }
        public string create_time { get; set; }
        public string reference { get; set; }
        public string description { get; set; }
        public QRCodeOP qr { get; set; }
    }
}
