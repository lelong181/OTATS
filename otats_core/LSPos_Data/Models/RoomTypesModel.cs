using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class RoomTypesModel
    {
        public int RoomTypeId { get; set; }
        public int HotelId { get; set; }
        public string TypeName { get; set; }
        public decimal BasePrice { get; set; }
        public int? MaxAdults { get; set; }
        public int? MaxChildren { get; set; }
        public decimal? AreaSquareMeter { get; set; }
        public string Description { get; set; }
        public int? ID_QLLH { get; set; }
    }
}
