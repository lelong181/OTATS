using Ticket.Facade;

namespace Ticket.Business
{
	public class SessionBO : BaseBO
	{
		private SessionFacade facade = SessionFacade.Instance;

		protected static SessionBO instance = new SessionBO();

		public static SessionBO Instance => instance;

		protected SessionBO()
		{
			baseFacade = facade;
		}
	}
}
