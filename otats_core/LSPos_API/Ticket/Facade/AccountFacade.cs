namespace Ticket.Facade
{
	public class AccountFacade : BaseFacade
	{
		protected static AccountFacade instance = new AccountFacade(new AccountModel());

		public static AccountFacade Instance => instance;

		protected AccountFacade(AccountModel model)
			: base(model)
		{
		}

		protected AccountFacade()
		{
		}
	}
}
