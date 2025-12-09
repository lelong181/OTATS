using Ticket.Facade;

namespace Ticket.Business
{
	public class ComputerBO : BaseBO
	{
		private ComputerFacade facade = ComputerFacade.Instance;

		protected static ComputerBO instance = new ComputerBO();

		public static ComputerBO Instance => instance;

		protected ComputerBO()
		{
			baseFacade = facade;
		}
	}
}
