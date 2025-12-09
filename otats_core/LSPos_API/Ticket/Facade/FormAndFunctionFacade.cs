namespace Ticket.Facade
{
	public class FormAndFunctionFacade : BaseFacade
	{
		protected static FormAndFunctionFacade instance = new FormAndFunctionFacade(new FormAndFunctionModel());

		public static FormAndFunctionFacade Instance => instance;

		protected FormAndFunctionFacade(FormAndFunctionModel model)
			: base(model)
		{
		}

		protected FormAndFunctionFacade()
		{
		}
	}
}
