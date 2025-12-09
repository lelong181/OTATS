using Ticket.Facade;

namespace Ticket.Business
{
	public class VersionsBO : BaseBO
	{
		private VersionsFacade facade = VersionsFacade.Instance;

		protected static VersionsBO instance = new VersionsBO();

		public static VersionsBO Instance => instance;

		protected VersionsBO()
		{
			baseFacade = facade;
		}
	}
}
