using System;

namespace Ticket
{
	public class BOException : Exception
	{
		public BOException(string message)
			: base(message)
		{
		}

		public BOException(string message, Exception e, string className)
		{
		}
	}
}
