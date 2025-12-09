using Ticket.Facade;

namespace Ticket.Business
{
	public class UsersBO : BaseBO
	{
		private UsersFacade facade = UsersFacade.Instance;

		protected static UsersBO instance = new UsersBO();

		public static UsersBO Instance => instance;

		protected UsersBO()
		{
			baseFacade = facade;
		}
	}
}
