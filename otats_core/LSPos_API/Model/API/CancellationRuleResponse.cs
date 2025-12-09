using System;

namespace BusinessLayer.Model.API
{
	public class CancellationRuleResponse
	{
		public Guid ID { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string DayOfWeek { get; set; }

		public int? CancellationDay { get; set; }

		public decimal CancelPercent { get; set; }

		public Guid SiteID { get; set; }
	}
}
