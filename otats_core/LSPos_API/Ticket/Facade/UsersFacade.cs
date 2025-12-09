namespace Ticket.Facade
{
	public class UsersFacade : BaseFacade
	{
		protected static UsersFacade instance = new UsersFacade(new UsersModel());

		public static UsersFacade Instance => instance;

		protected UsersFacade(UsersModel model)
			: base(model)
		{
		}

		protected UsersFacade()
		{
		}
	}
}
