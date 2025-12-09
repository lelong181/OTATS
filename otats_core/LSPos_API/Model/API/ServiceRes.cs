using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class ServiceRes
	{
		public string Id { get; set; }

		public string Type { get; set; }

		public bool IsCard { get; set; }

		public bool IsCaptured { get; set; }

		public bool IsShowImage { get; set; }

		public int NumberOfExpirationDay { get; set; }

		public bool OnlineSale { get; set; }

		public bool OfflineSale { get; set; }

		public decimal UnitPrice { get; set; }

		public int UnitQuantity { get; set; }

		public List<CommonI18n> I18ns { get; } = new List<CommonI18n>();

	}
}
