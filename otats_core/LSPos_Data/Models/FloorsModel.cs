using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class FloorsModel
    {
        public int FloorId { get; set; }
        public int BuildingId { get; set; }
        public string FloorNumber { get; set; }
        public string FloorName { get; set; }
        public string MapImage { get; set; }
        public int? ID_QLLH { get; set; }
    }
}
