using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class BuildingsModel
    {
        public int BuildingId { get; set; }
        public int ZoneId { get; set; }
        public string BuildingName { get; set; }
        public int? TotalFloors { get; set; }
        public bool? HasElevator { get; set; }
        public int? ID_QLLH { get; set; }
    }
}
