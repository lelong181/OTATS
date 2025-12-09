using System;

namespace BusinessLayer.Model.API
{
	public class TicketInfoRes
	{
		public Guid? AccountID { get; set; }

		public string ServiceRateName { get; set; }

		public string ServiceName { get; set; }

		public string ServiceCode { get; set; }

		public DateTime? IssuedDate { get; set; }

		public DateTime? ExpirationDate { get; set; }

		public string Status { get; set; }

		public int? PrintCount { get; set; }

		public string StatusStr { get; set; }

		public DateTime? SaleDate { get; set; }

		public int? NumberUsing { get; set; }

		public string LastUsingACM { get; set; }

		public DateTime? LastUsingTime { get; set; }

		public string Cashier { get; set; }

		public string BookingCode { get; set; }

		public string InvoiceCode { get; set; }

		public bool? PrintTicketBy { get; set; }

		public string CardType { get; set; }

		public Guid? InvoiceID { get; set; }

		public string ShiftName { get; set; }

		public DateTime? ShiftFromDate { get; set; }

		public string ZoneName { get; set; }

		public string ZoneGroupName { get; set; }

		public string SeatCode { get; set; }

		public string SeatNumber { get; set; }

		public string SeatType { get; set; }

		public string SerialNo { get; set; }

		public int ChangeCount { get; set; }
	}
}
