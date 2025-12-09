using Ticket.Facade;

namespace Ticket.Business
{
	public class LicenseBO : BaseBO
	{
		private LicenseFacade facade = LicenseFacade.Instance;

		protected static LicenseBO instance = new LicenseBO();

		public static LicenseBO Instance => instance;

		protected LicenseBO()
		{
			baseFacade = facade;
		}
	}
}
