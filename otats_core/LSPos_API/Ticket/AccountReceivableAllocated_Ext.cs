namespace Ticket
{
	public class AccountReceivableAllocated_Extension : AccountReceivableAllocated
	{
		public string ServiceCodeFrom { get; set; }

		public string ServiceNameFrom { get; set; }

		public string ServiceCodeTo { get; set; }

		public string ServiceNameTo { get; set; }

		public string UserFullName { get; set; }

		public string StatusStr { get; set; }
	}
}
