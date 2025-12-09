using Ticket.Facade;

namespace Ticket.Business
{
	public class FormAndFunctionBO : BaseBO
	{
		private FormAndFunctionFacade facade = FormAndFunctionFacade.Instance;

		protected static FormAndFunctionBO instance = new FormAndFunctionBO();

		public static FormAndFunctionBO Instance => instance;

		protected FormAndFunctionBO()
		{
			baseFacade = facade;
		}
	}
}
