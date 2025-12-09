using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class SmsBankingModel
    {
        public int ID { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RecieveDate { get; set; }
        public string Content { get; set; }
        public int ID_DonHang { get; set; }
        public int Amount { get; set; }
    }
}
