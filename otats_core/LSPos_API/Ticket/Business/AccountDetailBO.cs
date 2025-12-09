using Ticket.Facade;

namespace Ticket.Business
{
	public class AccountDetailBO : BaseBO
	{
		private AccountDetailFacade facade = AccountDetailFacade.Instance;

		protected static AccountDetailBO instance = new AccountDetailBO();

		public static AccountDetailBO Instance => instance;

		protected AccountDetailBO()
		{
			baseFacade = facade;
		}
	}
}
