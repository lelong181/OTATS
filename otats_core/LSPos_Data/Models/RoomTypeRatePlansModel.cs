using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class RoomTypeRatePlansModel
    {
        public long MappingId { get; set; }
        public int RoomTypeId { get; set; }
        public int RatePlanId { get; set; }
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public decimal DefaultPrice { get; set; }
        public bool? IsMappingActive { get; set; } // Creates alias IsActive for clarity if needed, but SP returns IsMappingActive
        public bool? IsBreakfastIncluded { get; set; }
        public bool? IsRefundable { get; set; }
        public int? ID_QLH { get; set; }
    }
}
