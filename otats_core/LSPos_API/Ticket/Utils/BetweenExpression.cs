using System;

namespace Ticket.Utils
{
	public class BetweenExpression : Expression
	{
		private string tmp;

		private object from;

		private object to;

		public object From => from;

		public object To => to;

		public BetweenExpression(string field, DateTime from, DateTime to)
		{
			this.from = from;
			this.to = to;
			setField(field);
		}

		public BetweenExpression(DateTime from, DateTime to)
			: this("FromDateTime", from, to)
		{
		}

		public void setField(string name)
		{
			tmp = string.Concat("('", From, "' <=", name, " AND ", name, "< '", To, "')");
		}

		public override string ToString()
		{
			return tmp;
		}

		public string ToString(string field)
		{
			setField(field);
			return tmp;
		}
	}
}
