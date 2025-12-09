using Ticket.Facade;

namespace Ticket.Business
{
	public class SystemInfoBO : BaseBO
	{
		private SystemInfoFacade facade = SystemInfoFacade.Instance;

		protected static SystemInfoBO instance = new SystemInfoBO();

		public static SystemInfoBO Instance => instance;

		protected SystemInfoBO()
		{
			baseFacade = facade;
		}
	}
}
