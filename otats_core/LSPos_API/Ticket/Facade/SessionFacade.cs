namespace Ticket.Facade
{
	public class SessionFacade : BaseFacade
	{
		protected static SessionFacade instance = new SessionFacade(new SessionModel());

		public static SessionFacade Instance => instance;

		protected SessionFacade(SessionModel model)
			: base(model)
		{
		}

		protected SessionFacade()
		{
		}
	}
}
