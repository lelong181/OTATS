namespace Ticket.Facade
{
	public class AccountDetailFacade : BaseFacade
	{
		protected static AccountDetailFacade instance = new AccountDetailFacade(new AccountDetailModel());

		public static AccountDetailFacade Instance => instance;

		protected AccountDetailFacade(AccountDetailModel model)
			: base(model)
		{
		}

		protected AccountDetailFacade()
		{
		}
	}
}
