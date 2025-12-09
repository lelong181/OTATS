using System.Collections.Generic;

namespace Ticket
{
	public class BookingModel : BaseModel
	{
		public int ChannelID { get; set; }

		public int BookingID { get; set; }

		public string BookingCode { get; set; }

		public string BookingName { get; set; }

		public string BookerName { get; set; }

		public string BookerEmail { get; set; }

		public string BookerPhone { get; set; }

		public string BookerIdentity { get; set; }

		public string UsingDate { get; set; }

		public string TotalAmount { get; set; }

		public List<ServiceModel> Services { get; set; }

		public string Status { get; set; }

		public BookingModel()
		{
			Services = new List<ServiceModel>();
		}
	}
}
