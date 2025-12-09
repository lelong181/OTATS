using Ticket.Facade;

namespace Ticket.Business
{
	public class ShiftBO : BaseBO
	{
		private ShiftFacade facade = ShiftFacade.Instance;

		protected static ShiftBO instance = new ShiftBO();

		public static ShiftBO Instance => instance;

		protected ShiftBO()
		{
			baseFacade = facade;
		}
	}
}
