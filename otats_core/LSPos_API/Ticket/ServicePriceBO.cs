using Ticket.Business;
using Ticket.Facade;

namespace Ticket
{
	public class ServicePriceBO : BaseBO
	{
		private ServicePriCDPcade facade = ServicePriCDPcade.Instance;

		protected static ServicePriceBO instance = new ServicePriceBO();

		public static ServicePriceBO Instance => instance;

		protected ServicePriceBO()
		{
			baseFacade = facade;
		}
	}
}
