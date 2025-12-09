using Ticket.Business;
using Ticket.Facade;

namespace Ticket
{
	public class ServiceBO : BaseBO
	{
		private ServiCDPcade facade = ServiCDPcade.Instance;

		protected static ServiceBO instance = new ServiceBO();

		public static ServiceBO Instance => instance;

		protected ServiceBO()
		{
			baseFacade = facade;
		}
	}
}
