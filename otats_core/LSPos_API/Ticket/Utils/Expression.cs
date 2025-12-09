namespace Ticket.Utils
{
	public class Expression
	{
		private object exp1;

		private object exp2;

		private string op;

		public Expression()
		{
		}

		public Expression(object exp1, object exp2)
			: this(exp1, exp2, "=")
		{
		}

		public Expression(string exp1, string exp2, string Operator)
			: this((object)exp1, (object)exp2, Operator)
		{
		}

		public Expression(string exp1, int exp2, string Operator)
			: this((object)exp1, (object)exp2, Operator)
		{
		}

		public Expression(object exp1, object exp2, string op)
		{
			if (exp1 is string)
			{
				this.exp1 = "$" + (string)exp1;
			}
			else
			{
				this.exp1 = exp1;
			}
			if (op.Equals("IN"))
			{
				this.exp2 = string.Concat("(", exp2, ")");
			}
			else
			{
				this.exp2 = exp2;
			}
			this.op = op;
		}

		public Expression And(Expression exp2)
		{
			return new Expression(this, exp2, "AND");
		}

		public Expression Or(Expression exp2)
		{
			return new Expression(this, exp2, "OR");
		}

		public override string ToString()
		{
			return ToString(exp1) + " " + op + " " + ToString(exp2);
		}

		private string ToString(object exp)
		{
			if (exp == null)
			{
				return " ";
			}
			if (exp is string)
			{
				string tmp = exp.ToString();
				tmp = tmp;
				if (tmp.StartsWith("$"))
				{
					return tmp.Substring(1);
				}
				if (tmp.StartsWith("("))
				{
					return tmp;
				}
				return "N'" + exp.ToString().Replace("'", "''") + "'";
			}
			if (exp is Expression)
			{
				return "(" + exp.ToString() + ")";
			}
			return exp.ToString();
		}
	}
}
