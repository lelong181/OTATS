using Ticket.Facade;

namespace Ticket.Business
{
	public class FormAndFunctionGroupBO : BaseBO
	{
		private FormAndFunctionGroupFacade facade = FormAndFunctionGroupFacade.Instance;

		protected static FormAndFunctionGroupBO instance = new FormAndFunctionGroupBO();

		public static FormAndFunctionGroupBO Instance => instance;

		protected FormAndFunctionGroupBO()
		{
			baseFacade = facade;
		}
	}
}
