using Ticket.Facade;

namespace Ticket.Business
{
	public class ProfileBO : BaseBO
	{
		private ProfileFacade facade = ProfileFacade.Instance;

		protected static ProfileBO instance = new ProfileBO();

		public static ProfileBO Instance => instance;

		protected ProfileBO()
		{
			baseFacade = facade;
		}
	}
}
