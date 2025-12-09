using Ticket.Facade;

namespace Ticket.Business
{
	public class InvVoucherBO : BaseBO
	{
		private InvVoucherFacade facade = InvVoucherFacade.Instance;

		protected static InvVoucherBO instance = new InvVoucherBO();

		public static InvVoucherBO Instance => instance;

		protected InvVoucherBO()
		{
			baseFacade = facade;
		}
	}
}
