using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class DailyRatesModel
    {
        public long DailyRateId { get; set; }
        public int RoomTypeId { get; set; }
        public int RatePlanId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int? Allotment { get; set; }
        public bool? IsClosed { get; set; }
        public int? ID_QLLH { get; set; }
    }
}
