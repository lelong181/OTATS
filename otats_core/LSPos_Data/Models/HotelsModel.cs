using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class HotelsModel
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }
        public bool? IsActive { get; set; }
        public int? ID_QLLH { get; set; }

        public ZonesModel Zone { get; set; } = new ZonesModel();
        public BuildingsModel Building { get; set; } = new BuildingsModel();
        public FloorsModel Floor { get; set; } = new FloorsModel();
        public RoomsModel Room { get; set; } = new RoomsModel();
        public RoomTypesModel RoomType { get; set; } = new RoomTypesModel();
    }
}
