using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingRateRes : ICloneable
	{
		public string Id { get; set; }

		public List<BookingDetailRes> Details { get; set; }

		public decimal Price { get; set; }

		public bool TaxIncluded { get; set; }

		public int Quantity { get; set; }

		public List<CommonI18n> I18ns { get; set; }

		public decimal Amount { get; set; }

		public string ServiceRateName { get; set; }

		public Guid? PromotionLinkID { get; set; }

		public Guid? PromotionID { get; set; }

		public BookingRateRes()
		{
			Details = new List<BookingDetailRes>();
			I18ns = new List<CommonI18n>();
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
