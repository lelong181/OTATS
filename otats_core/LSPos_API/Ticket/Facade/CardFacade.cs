namespace Ticket.Facade
{
	public class CardFacade : BaseFacade
	{
		protected static CardFacade instance = new CardFacade(new CardModel());

		public static CardFacade Instance => instance;

		protected CardFacade(CardModel model)
			: base(model)
		{
		}

		protected CardFacade()
		{
		}
	}
}
