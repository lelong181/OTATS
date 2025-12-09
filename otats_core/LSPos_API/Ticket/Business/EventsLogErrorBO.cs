using Ticket.Facade;

namespace Ticket.Business
{
	public class EventsLogErrorBO : BaseBO
	{
		private EventsLogErrorFacade facade = EventsLogErrorFacade.Instance;

		protected static EventsLogErrorBO instance = new EventsLogErrorBO();

		public static EventsLogErrorBO Instance => instance;

		protected EventsLogErrorBO()
		{
			baseFacade = facade;
		}
	}
}
