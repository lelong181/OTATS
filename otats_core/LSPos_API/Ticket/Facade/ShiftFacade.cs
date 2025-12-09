namespace Ticket.Facade
{
	public class ShiftFacade : BaseFacade
	{
		protected static ShiftFacade instance = new ShiftFacade(new ShiftModel());

		public static ShiftFacade Instance => instance;

		protected ShiftFacade(ShiftModel model)
			: base(model)
		{
		}

		protected ShiftFacade()
		{
		}
	}
}
