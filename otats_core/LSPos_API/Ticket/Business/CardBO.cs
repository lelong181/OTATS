using Ticket.Facade;

namespace Ticket.Business
{
	public class CardBO : BaseBO
	{
		private CardFacade facade = CardFacade.Instance;

		protected static CardBO instance = new CardBO();

		public static CardBO Instance => instance;

		protected CardBO()
		{
			baseFacade = facade;
		}
	}
}
