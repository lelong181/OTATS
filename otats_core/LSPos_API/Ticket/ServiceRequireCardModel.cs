using System.Collections.Generic;

namespace Ticket
{
	public class ServiceRequireCardModel
	{
		public int ServiceID { get; set; }

		public int ServiceTypeID { get; set; }

		public string ServiceName { get; set; }

		public int QuantityNeeded { get; set; }

		public int QuantityCurrent { get; set; }

		public List<string> ListCardID { get; set; }

		public ServiceRequireCardModel()
		{
			ListCardID = new List<string>();
		}
	}
}
