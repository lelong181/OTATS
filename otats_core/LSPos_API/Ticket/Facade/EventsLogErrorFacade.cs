namespace Ticket.Facade
{
	public class EventsLogErrorFacade : BaseFacade
	{
		protected static EventsLogErrorFacade instance = new EventsLogErrorFacade(new EventsLogErrorModel());

		public static EventsLogErrorFacade Instance => instance;

		protected EventsLogErrorFacade(EventsLogErrorModel model)
			: base(model)
		{
		}

		protected EventsLogErrorFacade()
		{
		}
	}
}
