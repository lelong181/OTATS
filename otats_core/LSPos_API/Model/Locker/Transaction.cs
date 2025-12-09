namespace BusinessLayer.Model.Locker
{
	public class Transaction
	{
		public string CardID { get; set; }

		public int ZoneID { get; set; }

		public int LockerID { get; set; }

		public int LineID { get; set; }

		public string StartDate { get; set; }

		public string EndDate { get; set; }

		public int Status { get; set; }

		public string lockerName { get; set; }

		public string lineName { get; set; }

		public string zoneName { get; set; }
	}
}
