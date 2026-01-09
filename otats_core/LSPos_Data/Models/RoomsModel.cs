using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class RoomsModel
    {
        public long RoomId { get; set; }
        public int FloorId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeId { get; set; }
        public string Status { get; set; }
        public string CleanStatus { get; set; }
        public bool? IsSmokingAllowed { get; set; }
        public bool? IsConnectingRoom { get; set; }
        public long? ConnectedRoomId { get; set; }
        public int? ID_QLLH { get; set; }

        public RoomTypesModel RoomType { get; set; }
        public FloorsModel Floor { get; set; }
        public BuildingsModel Building { get; set; }
    }
}
