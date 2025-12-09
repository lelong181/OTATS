using System;

namespace BusinessLayer.Model.API
{
	public class AccountResponse
	{
		public string AccountCode { get; set; }

		public DateTime IssuedDate { get; set; }

		public string InvoiceCode { get; set; }

		public string Title { get; set; }

		public bool? IsMasterCode { get; set; }

		public int NumberOfUses { get; set; }

		public string Description { get; set; }

		public string Cashier { get; set; }

		public string Status { get; set; }

		public DateTime UsingDate { get; set; }

		public DateTime? ExpirationDate { get; set; }

		public string UsingDateStr { get; set; }

		public string ComputerName { get; set; }

		public string Classify { get; set; }

		public int Sequence { get; set; }

		public byte[] Image { get; set; }

		public string ServiceGroupName { get; set; }

		public string ServiceName { get; set; }

		public decimal TotalMoney { get; set; }

		public string StatusStr { get; set; }

		public string AccountID { get; set; }

		public string InvoiceID { get; set; }

		public int PrintCount { get; set; }

		public string FlexCol1 { get; set; }

		public bool? PrintTicketBy { get; set; }

		public string ShiftName { get; set; }

		public DateTime? ShiftFromDate { get; set; }

		public string ZoneName { get; set; }

		public string ZoneGroupName { get; set; }

		public string SeatCode { get; set; }

		public string SeatNumber { get; set; }

		public string SeatType { get; set; }

		public string SerialNo { get; set; }

		public string DescriptionSvRate { get; set; }

		public bool? IsHideOnTicket { get; set; }
	}
}
