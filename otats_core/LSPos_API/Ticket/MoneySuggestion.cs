using System;
using System.Collections.Generic;
using System.Linq;

namespace Ticket
{
	public class MoneySuggestion
	{
		public static List<long> MoneyPayCases(long Price)
		{
			if (Price < 1000)
			{
				return new List<long> { 1000L };
			}
			long IsRoundNeeded = Price % 1000;
			List<long> FirstRs = new List<long>();
			Price = (long)Math.Ceiling((double)Price / 1000.0);
			if (IsRoundNeeded > 0)
			{
				FirstRs.Add(Price);
			}
			long Div_12 = Price / 1000;
			long Div_11 = Price / 100 % 10;
			long Div_10 = Price / 10;
			long Mod_101 = Price % 1000;
			long Mod_100 = Price % 100;
			List<long> Lst_10 = new List<long> { 10L, 20L, 50L, 100L };
			List<long> Lst_11 = new List<long> { 100L, 200L, 500L, 1000L };
			IEnumerable<long> Lst_Rs_10 = new List<long>();
			if (Mod_100 != 0)
			{
				Lst_Rs_10 = Lst_10.Where((long x) => x >= Mod_100);
				Lst_Rs_10 = Lst_Rs_10.Select((long x) => Div_12 * 1000 + Div_11 * 100 + x);
			}
			IEnumerable<long> Lst_Rs_11 = Lst_11.Where((long x) => x >= Mod_101);
			Lst_Rs_11 = Lst_Rs_11.Select((long x) => Div_12 * 1000 + x);
			IOrderedEnumerable<long> Lst_Rs = from x in FirstRs.Concat(Lst_Rs_10.Concat(Lst_Rs_11)).Distinct()
				orderby x
				select x;
			return Lst_Rs.Select((long x) => x * 1000).ToList();
		}
	}
}
