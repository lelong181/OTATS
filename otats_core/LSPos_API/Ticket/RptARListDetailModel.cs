using System.Collections.Generic;

namespace Ticket
{
	public class RptARListDetailModel
	{
		public IEnumerable<RptARListDetail> list { get; set; }

		public IEnumerable<RptARListDetailAllocated> details { get; set; }
	}
}
