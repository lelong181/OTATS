using Ticket.Business;
using Ticket.Facade;

namespace Ticket
{
	public class PropertyBO : BaseBO
	{
		private PropertyFacade facade = PropertyFacade.Instance;

		protected static PropertyBO instance = new PropertyBO();

		public static PropertyBO Instance => instance;

		protected PropertyBO()
		{
			baseFacade = facade;
		}
	}
}
