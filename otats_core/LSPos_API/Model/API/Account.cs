using System;

namespace BusinessLayer.Model.API
{
	public class Account
	{
		public Guid ID { get; set; }

		public string AccountCode { get; set; }
		public string BookingCode { get; set; }

		public Guid? ServiceID { get; set; }

		public Guid? BookingDetailID { get; set; }

		public string CardID { get; set; }

		public Guid? EmployeeID { get; set; }

		public Guid? MemberID { get; set; }

		public Guid? BookingID { get; set; }

		public DateTime? IssuedDate { get; set; }

		public bool IsMasterCode { get; set; }

		public DateTime? ExpirationDate { get; set; }

		public decimal TotalMoney { get; set; }

		public string Status { get; set; }

		public string StatusStr { get; set; }

		public decimal Balance { get; set; }

		public int PrintCount { get; set; }

		public string SiteCode { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
	}
}
