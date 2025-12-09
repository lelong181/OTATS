using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common.Paycollect
{
    public class AccountOP
    {
        public string id {  get; set; }
        public string account_number {  get; set; }
        public string bank_name {  get; set; }
        public string swift_code {  get; set; }
        public string state {  get; set; }
        public QRCodeOP qr { get; set; }
    }
}
