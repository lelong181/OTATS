using System.Collections.Generic;

namespace Ticket
{
	public class DetailAR
	{
		public IEnumerable<AccountReceivableAllocated_Extension> ListARAllocated { get; set; }

		public ARTrans AR { get; set; }

		public DetailAR()
		{
			AR = new ARTrans();
		}
	}
}
