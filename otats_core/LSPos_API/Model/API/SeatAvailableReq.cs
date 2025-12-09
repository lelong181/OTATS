namespace BusinessLayer.Model.API
{
	public class SeatAvailableReq
	{
		public string LangCode { get; set; }

		public string ShiftID { get; set; }

		public string ZoneID { get; set; }

		public string ServiceRateID { get; set; }

		public string Type { get; set; }
	}
}
