namespace Ticket.Facade
{
	public class ServicePriCDPcade : BaseFacade
	{
		protected static ServicePriCDPcade instance = new ServicePriCDPcade(new ServicePriceModel());

		public static ServicePriCDPcade Instance => instance;

		protected ServicePriCDPcade(ServicePriceModel model)
			: base(model)
		{
		}

		protected ServicePriCDPcade()
		{
		}
	}
}
