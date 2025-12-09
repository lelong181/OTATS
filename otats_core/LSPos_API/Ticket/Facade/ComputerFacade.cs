namespace Ticket.Facade
{
	public class ComputerFacade : BaseFacade
	{
		protected static ComputerFacade instance = new ComputerFacade(new ComputerModel());

		public static ComputerFacade Instance => instance;

		protected ComputerFacade(ComputerModel model)
		{
		}

		protected ComputerFacade()
		{
		}
	}
}
