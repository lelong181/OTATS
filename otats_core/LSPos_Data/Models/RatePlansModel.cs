using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class RatePlansModel
    {
        public int RatePlanId { get; set; }
        public int HotelId { get; set; }
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public bool? IsBreakfastIncluded { get; set; }
        public bool? IsLunchIncluded { get; set; }
        public bool? IsDinnerIncluded { get; set; }
        public int? CancellationPolicyId { get; set; }
        public bool? IsRefundable { get; set; }
        public int? MinLengthOfStay { get; set; }
        public int? MaxLengthOfStay { get; set; }
        public bool? IsActive { get; set; }
        public int? ID_QLLH { get; set; }
    }
}
