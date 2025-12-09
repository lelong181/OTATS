using System;

namespace BusinessLayer.Model.API
{
	public class CancelRes
	{
		public Guid ID { get; set; }

		public int? CancellationDay { get; set; }

		public decimal CancelPercent { get; set; }
	}
}
