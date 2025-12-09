using System;

namespace BusinessLayer.Model.API
{
	public class AccountPrepareUpgrade
	{
		public Guid AccountID { get; set; }

		public string CardID { get; set; }

		public string AccountCode { get; set; }

		public DateTime? IssuedDate { get; set; }

		public DateTime? ExpirationDate { get; set; }

		public decimal? TotalMoney { get; set; }

		public string CardType { get; set; }

		public Guid? ServiceID { get; set; }

		public string ServiceName { get; set; }

		public Guid? ServiceRateID { get; set; }

		public string Status { get; set; }

		public string StatusStr { get; set; }

		public string ServiceRateName { get; set; }

		public Guid? BookingDetailSeatID { get; set; }
	}
}
