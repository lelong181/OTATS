using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingDetailRes
	{
		public int Quantity { get; set; }

		public int UnitQuantity { get; set; }

		public decimal Amount { get; set; }

		public bool IsPromotion { get; set; }

		public bool IsSplit { get; set; }

		public int RowState { get; set; }

		public string Description { get; set; }

		public string GroupBy { get; set; }

		public string TransactionNo { get; set; }

		public Promotion promotion { get; set; }

		public List<CommonI18n> I18ns { get; set; }
	}
}
