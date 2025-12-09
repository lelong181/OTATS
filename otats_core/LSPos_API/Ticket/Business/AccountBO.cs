using Ticket.Facade;

namespace Ticket.Business
{
	public class AccountBO : BaseBO
	{
		private AccountFacade facade = AccountFacade.Instance;

		protected static AccountBO instance = new AccountBO();

		public static AccountBO Instance => instance;

		protected AccountBO()
		{
			baseFacade = facade;
		}
	}
}
