using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingAccountResponse
	{
		public string AccountCode { get; set; }

		public string ServiceRateID { get; set; }

		public string InvoiceCode { get; set; }

		public DateTime ExpirationDate { get; set; }
		public DateTime DateUsing { get; set; }

		public DateTime? ExpiredDate { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public List<CommonI18n> I18ns { get; set; }

		public int NumberOfUses { get; set; }

		public bool? IsMasterCode { get; set; }

		public int Sequence { get; set; }

		public string Classify { get; set; }

		public byte[] Image { get; set; }

		public decimal Price { get; set; }

		public string FlexCol1 { get; set; }

		public string SerialNo { get; set; }

		public string SeatType { get; set; }

		public string SeatCode { get; set; }

		public string SeatNumber { get; set; }

		public string ZoneGroupName { get; set; }

		public string ZoneName { get; set; }

		public bool? IsHideOnTicket { get; set; }

		public string DescriptionSvRate { get; set; }
	}
}
