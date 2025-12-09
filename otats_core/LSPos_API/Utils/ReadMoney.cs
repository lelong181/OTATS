namespace BusinessLayer.Utils
{
	public class ReadMoney
	{
		public static string MoneyText(string number)
		{
			string[] dv = new string[6] { "", "mươi", "trăm", "ngàn", "triệu", "tỉ" };
			string[] cs = new string[10] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
			int len = number.Length;
			number += "ss";
			string doc = "";
			int found = 0;
			int ddv = 0;
			int rd = 0;
			int l;
			for (int i = 0; i < len; i += l)
			{
				l = (len - i + 2) % 3 + 1;
				found = 0;
				for (int j = 0; j < l; j++)
				{
					if (number[i + j] != '0')
					{
						found = 1;
						break;
					}
				}
				if (found == 1)
				{
					rd = 1;
					for (int j = 0; j < l; j++)
					{
						ddv = 1;
						switch (number[i + j])
						{
						case '0':
							if (l - j == 3)
							{
								doc = doc + cs[0] + " ";
							}
							if (l - j == 2)
							{
								if (number[i + j + 1] != '0')
								{
									doc += "lẻ ";
								}
								ddv = 0;
							}
							break;
						case '1':
							if (l - j == 3)
							{
								doc = doc + cs[1] + " ";
							}
							if (l - j == 2)
							{
								doc += "mười ";
								ddv = 0;
							}
							if (l - j == 1)
							{
								int k = ((i + j != 0) ? (i + j - 1) : 0);
								doc = ((number[k] == '1' || number[k] == '0') ? (doc + cs[1] + " ") : (doc + "mốt "));
							}
							break;
						case '5':
							doc = ((i + j != len - 1) ? (doc + cs[5] + " ") : (doc + "lăm "));
							break;
						default:
							doc = doc + cs[number[i + j] - 48] + " ";
							break;
						}
						if (ddv == 1)
						{
							doc += dv[l - j - 1];
						}
					}
				}
				if (len - i - l <= 0)
				{
					continue;
				}
				if ((len - i - l) % 9 == 0)
				{
					if (rd == 1)
					{
						for (int k = 0; k < (len - i - l) / 9; k++)
						{
							doc += " tỉ ";
						}
					}
					rd = 0;
				}
				else if (found != 0)
				{
					doc = doc + " " + dv[(len - i - l + 1) % 9 / 3 + 2];
				}
			}
			if (len == 1 && (number[0] == '0' || number[0] == '5'))
			{
				return cs[number[0] - 48];
			}
			return doc;
		}
	}
}
