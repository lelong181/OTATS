using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class ZonesModel
    {
        public int ZoneId { get; set; }
        public int HotelId { get; set; }
        public string ZoneName { get; set; }
        public string Description { get; set; }
        public int? DistanceToReception { get; set; }
        public int? ID_QLLH { get; set; }
    }
}
