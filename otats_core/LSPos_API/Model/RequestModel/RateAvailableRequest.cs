using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_API.Model.RequestModel
{
    public class RateAvailableReq
    {
        public string SiteId { get; set; }

        public string ProfileId { get; set; }

        public string CheckIn { get; set; }

        public string Channel { get; set; }

        public Guid? ShiftID { get; set; }

        public Guid GetProfileId()
        {
            Guid parsed;
            return Guid.TryParse(ProfileId, out parsed) ? parsed : Guid.Empty;
        }

        public Guid GetSiteId()
        {
            return Guid.Parse(SiteId);
        }

        public DateTime GetCheckIn()
        {
            return DateTime.Parse(CheckIn);
        }
    }
}
