namespace Ticket.Facade
{
	public class FormAndFunctionGroupFacade : BaseFacade
	{
		protected static FormAndFunctionGroupFacade instance = new FormAndFunctionGroupFacade(new FormAndFunctionGroupModel());

		public static FormAndFunctionGroupFacade Instance => instance;

		protected FormAndFunctionGroupFacade(FormAndFunctionGroupModel model)
			: base(model)
		{
		}

		protected FormAndFunctionGroupFacade()
		{
		}
	}
}
