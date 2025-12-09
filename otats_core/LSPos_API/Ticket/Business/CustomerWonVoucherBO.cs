using Ticket.Facade;

namespace Ticket.Business
{
	public class CustomerWonVoucherBO : BaseBO
	{
		private CustomerWonVoucherFacade facade = CustomerWonVoucherFacade.Instance;

		protected static CustomerWonVoucherBO instance = new CustomerWonVoucherBO();

		public static CustomerWonVoucherBO Instance => instance;

		protected CustomerWonVoucherBO()
		{
			baseFacade = facade;
		}
	}
}
