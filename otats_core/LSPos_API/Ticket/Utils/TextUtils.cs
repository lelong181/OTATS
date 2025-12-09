// Ticket.Utils.TextUtils
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Grid;
using Ticket;
using Ticket.Business;
using Ticket.Utils;
using DataTable = System.Data.DataTable;

namespace Ticket.Utils
{
	public class TextUtils
	{
		public const string Version = "1.0.090918.1";

		public const int MaxCategory = 6;

		public const int MaxServiceItem = 6;

		public const decimal _ServiceChargePercentage = 10m;

		public const decimal _ServiceChargeAmount = 0m;

		public const int MaxFolio = 8;

		public const string ColorCurrency = "Blue";

		public const string CurrencyFormat = "###,###,###,###.00";

		public const string CurrencyFormatVND = "###,###,###,###";

		public const string FormatShortDate = "dd/MM/yyyy";

		public const string FomatShortDate = "dd/MM/yyyy";

		public const string FormatLongDate = "dd/MM/yyyy HH:mm";

		public const string ConStr = "Server=.;database=CSSERP;uid=sa;pwd=";

		public const int AutoRefreshReport = 60000;

		public const string LoadDataError = "Không load được dữ liệu";

		public const string InsertSucessful = "Thêm mới dữ liệu thành công";

		public const string UpdateSucessful = "Điều chỉnh dữ liệu thành công";

		public const string DeleteSucessful = "Xóa dữ liệu thành công";

		public const string Choice = "Bạn có chắc bạn muốn xóa dòng dữ liệu này không?";

		public const string Caption = "Thông báo";

		public const string Caption_Message = "Thông báo";

		public const string ErrorCaption = "Thông báo lỗi";

		public const string Logo = "";

		public const string InsertFail = "Có lỗi xẩy ra khi thêm mới dữ liệu";

		public const string UpdateFail = "Có lỗi xẩy ra khi điều chỉnh dữ liệu";

		public const string DeleteFail = "Có lỗi xẩy ra khi xóa dữ liệu";

		public const string SelectRow = " Bạn phải chọn dòng dữ liệu để xóa";

		public const string Entity_List = "Select * from [EntityObj]";

		public const string Entity_Delete = "Delete from [EntityObj] where Field1=@Field1";

		public const string Entity_Add = "Delete from [EntityObj] where Field1=@Field1";

		private static DateTime EMPTY_DATE = new DateTime(1, 1, 1);

		private static DateTime CSS_MAX_DATE = DateTime.Parse("1/1/2099");

		private static DateTime MIN_DATE = DateTime.MinValue;

		private static DateTime MAX_DATE = DateTime.MaxValue;

		public static DateTimeFormatInfo VN_DTFI = new CultureInfo("fr-FR", useUserOverride: false).DateTimeFormat;

		private static string[] Number_Patterns = new string[6] { "{0:#,##0}", "{0:#,##0.0}", "{0:#,##0.00}", "{0:#,##0}.000", "{0:#,##0.0000}", "{0:#,##0.00000;#,##0.00000; }" };

		private static string[] Currency_Patterns = new string[6] { "{0:$#,##0;($#,##0); }", "{0:$#,##0.0;($#,##0.0); }", "{0:$#,##0.00;($#,##0.00); }", "{0:$#,##0.000;($#,##0.000); }", "{0:$#,##0.0000;($#,##0.0000); }", "{0:$#,##0.00000;($#,##0.00000); }" };

		private static SqlConnection mySqlConnection;

		public static bool Log(string U, string P, ref UsersModel mU)
		{
			try
			{
				Expression exp = new Expression("LoginName", U, "=");
				exp = exp.And(new Expression("PassWordHash", MD5.Hash(P), "="));
				ArrayList arrU = UsersBO.Instance.FindByExpression(exp);
				if (arrU != null && arrU.Count > 0)
				{
					mU = (UsersModel)arrU[0];
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static int NumberOfDay(string Interval, DateTime Date1, DateTime Date2)
		{
			int intDateDiff = 0;
			TimeSpan time = Date1 - Date2;
			int timeHours = Math.Abs(time.Hours);
			int timeDays = Math.Abs(time.Days);
			switch (Interval.ToLower())
			{
				case "h":
					intDateDiff = timeHours;
					break;
				case "d":
					intDateDiff = Math.Abs((Date1.Date - Date2.Date).Days);
					break;
				case "w":
					intDateDiff = timeDays / 7;
					break;
				case "bw":
					intDateDiff = timeDays / 7 / 2;
					break;
				case "m":
					timeDays -= timeDays / 365 * 5;
					intDateDiff = timeDays / 30;
					break;
				case "bm":
					timeDays -= timeDays / 365 * 5;
					intDateDiff = timeDays / 30 / 2;
					break;
				case "q":
					timeDays -= timeDays / 365 * 5;
					intDateDiff = timeDays / 90;
					break;
				case "y":
					intDateDiff = timeDays / 365;
					break;
			}
			return intDateDiff;
		}

		public static string ToUpperFC(string str)
		{
			return str.Substring(0, 1).ToUpper() + str.Substring(1);
		}

		public static string ToString(object obj)
		{
			return obj?.ToString();
		}

		public static int ToInt(object x)
		{
			try
			{
				return Convert.ToInt32(x);
			}
			catch
			{
				return 0;
			}
		}

		public static int ToInt(string x)
		{
			try
			{
				return int.Parse(x);
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public static long ToInt64(object x)
		{
			try
			{
				return Convert.ToInt64(x);
			}
			catch
			{
				return 0L;
			}
		}

		public static long ToInt64(string x)
		{
			try
			{
				return long.Parse(x);
			}
			catch (Exception)
			{
				return 0L;
			}
		}

		public static decimal ToDecimal(object x)
		{
			try
			{
				return Convert.ToDecimal(x);
			}
			catch
			{
				return 0m;
			}
		}

		public static decimal ToDecimal(string x)
		{
			try
			{
				return decimal.Parse(x);
			}
			catch (Exception)
			{
				return 0m;
			}
		}

		public static double ToDouble(object x)
		{
			try
			{
				return Convert.ToDouble(x);
			}
			catch
			{
				return 0.0;
			}
		}

		public static double ToDouble(string x)
		{
			try
			{
				return double.Parse(x);
			}
			catch (Exception)
			{
				return 0.0;
			}
		}

		public static bool ToBoolean(object x)
		{
			try
			{
				return Convert.ToBoolean(x);
			}
			catch
			{
				return false;
			}
		}





		public static string NumericToString(decimal Num)
		{
			string[] Dvi = new string[5] { "", "ngàn", "triệu", "tỷ", "ngàn" };
			string Result = "";
			long IntNum = (long)Num;
			for (int i = 0; i < 5; i++)
			{
				long Doc = IntNum % 1000;
				if (Doc > 0)
				{
					Result = NumericToString(Doc, IntNum < 1000) + " " + Dvi[i] + " " + Result;
				}
				IntNum /= 1000;
			}
			Result = Result + "đồng" + (((long)Num % 1000 == 0L) ? " chẵn" : "");
			if (Result.ToString().Trim().Substring(0, 1) == "m" && Result.ToString().Trim().Substring(3, 1) == "i")
			{
				Result = "Mười " + Result.Remove(0, 5);
			}
			return Result;
		}

		public static string DoiDau(string strNumber)
		{
			int length = 0;
			int pos = 0;
			string st = "";
			string DoiDau1 = "";
			st = strNumber;
			for (pos = st.IndexOf(".", 0); pos > 0; pos = st.IndexOf(".", 0))
			{
				st = st.Substring(0, pos) + st.Remove(0, pos);
			}
			length = st.Length;
			pos = st.IndexOf(",", 0);
			if (pos > 0)
			{
				return st.Substring(0, pos) + "." + st.Remove(0, pos + 1);
			}
			return st;
		}

		public static string FormatDate(DateTime date, string format)
		{
			if (date.Year < 1000 || date.Year >= 2099)
			{
				return "N/A";
			}
			return date.ToString(format, DateTimeFormatInfo.InvariantInfo);
		}

		public static string FormatDate(DateTime date)
		{
			return FormatDate(date, "MMM dd, yyyy");
		}

		public static string FormatDateTime(DateTime date)
		{
			return FormatDate(date, "MMM dd, yyyy HH:mm:ss");
		}

		public static string FormatDateToMonthAndYear(string date)
		{
			return FormatDate(ToDate(date), "MMM, yyyy");
		}

		public static string FormatDate(string date)
		{
			return FormatDate(ToDate(date), "MMM dd, yyyy");
		}

		public static DateTime ToDate(string date)
		{
			return DateTime.Parse(date, new CultureInfo("en-US", useUserOverride: true));
		}

		public static DateTime ToDate(object date, string shortDatePattern, string shortTimePattern = null, string fullDateTimePattern = null)
		{
			CultureInfo cul = new CultureInfo("en-US");
			if (shortDatePattern != null)
			{
				cul.DateTimeFormat.ShortDatePattern = shortDatePattern;
			}
			if (shortTimePattern != null)
			{
				cul.DateTimeFormat.ShortTimePattern = shortTimePattern;
			}
			if (fullDateTimePattern != null)
			{
				cul.DateTimeFormat.FullDateTimePattern = fullDateTimePattern;
			}
			return Convert.ToDateTime(date, cul);
		}

		public static string ToString1(DateTime date)
		{
			return date.ToString("MM/dd/yyyy", new CultureInfo("en-US", useUserOverride: true));
		}

		public static bool UpdateByCommand(string command)
		{
			SqlConnection cnn = new SqlConnection(DBUtils.GetDBConnectionString());
			bool update = false;
			try
			{
				SqlCommand cmd = new SqlCommand();
				cnn.Open();
				cmd = new SqlCommand("spSearchAllForTrans", cnn);
				cmd.CommandTimeout = 6000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", command));
				cmd.ExecuteNonQuery();
				update = true;
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cnn.Close();
			}
			return update;
		}

		public static string GetFullMonth(DateTime date)
		{
			return FormatDate(date, "MMMM");
		}

		public static string FormatNumber(object x, int digits)
		{
			try
			{
				return string.Format(Number_Patterns[digits], x);
			}
			catch
			{
				return "0";
			}
		}

		public static string FormatNumber(decimal x, int digits)
		{
			return string.Format(Number_Patterns[digits], x);
		}

		public static string FormatCurrency(decimal x, int digits)
		{
			return string.Format(Currency_Patterns[digits], x);
		}

		public static string FormatNumberZeroToDash(decimal x)
		{
			return "{0:##;-##;-}";
		}

		public static string FormatPercentNumber(decimal x)
		{
			return $"{x:#0.00}%";
		}

		public static ArrayList SplitPrefixes(string rawPref)
		{
			ArrayList prefList = new ArrayList();
			try
			{
				string[] prefParts = rawPref.Split(';');
				for (int i = 0; i < prefParts.Length; i++)
				{
					string[] temp = prefParts[i].Split('-');
					if (temp.Length == 1)
					{
						if (!prefList.Contains(temp[0].Trim()))
						{
							prefList.Add(temp[0].Trim());
						}
					}
					else if (temp.Length == 2)
					{
						int noOfPrefs = int.Parse(temp[1]) - int.Parse(temp[0]);
						for (int j = 0; j <= noOfPrefs; j++)
						{
							if (!prefList.Contains((int.Parse(temp[0]) + j).ToString().Trim()))
							{
								prefList.Add((int.Parse(temp[0]) + j).ToString().Trim());
							}
						}
					}
					else
					{
						prefList.Clear();
					}
				}
			}
			catch
			{
				return prefList;
			}
			return prefList;
		}

		public static string[] SplitBillingIncrement(string rawIncr, out bool isValid)
		{
			isValid = false;
			string[] incrParts = rawIncr.Split('+');
			if (incrParts.Length != 2)
			{
				return incrParts;
			}
			for (int i = 0; i < incrParts.Length; i++)
			{
				if (!double.TryParse(incrParts[i].ToString(), NumberStyles.Number, NumberFormatInfo.CurrentInfo, out var result))
				{
					return incrParts;
				}
				if (result != (double)int.Parse(result.ToString()) || result <= 0.0 || result > 60.0)
				{
					return incrParts;
				}
			}
			isValid = true;
			return incrParts;
		}

		public static string Error(string msg)
		{
			return "ERROR: " + msg;
		}

		public static string Warn(string msg)
		{
			return "WARNING: " + msg;
		}

		public static string ToHTML(NameValueCollection list)
		{
			StringBuilder r = new StringBuilder();
			string[] allKeys = list.AllKeys;
			foreach (string key in allKeys)
			{
				r.Append("<span class=subtitle2>" + key + "</span>: " + list.Get(key) + "<br>");
			}
			return r.ToString();
		}

		public static bool IsEmail(string str)
		{
			bool State = true;
			if (!Regex.IsMatch(str, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
			{
				State = false;
			}
			return State;
		}

		private static string NumberToString(long Num)
		{
			string[] Number = new string[10] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
			return Number[Num];
		}

		public static string NumberToStringH(long Num)
		{
			string[] Number = new string[10] { "không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín" };
			return Number[Num];
		}

		private static string NumericToString(long Num, bool Dau)
		{
			long Tram = Num / 100;
			long Chuc = Num % 100 / 10;
			long Dvi = Num % 10;
			string v1 = "";
			switch (Chuc) {
				case 1L:
					v1= " mươi";
					break;
				case 0L: v1=(Tram == 0 && Dau) ? "" : ((Dvi == 0L) ? "" : " lẻ");
					break;
				default: v1= " " + NumberToString(Chuc) + " mươi";
					break;
			}
            string v = v1;
            return ((Tram == 0 && Dau) ? "" : (" " + NumberToString(Tram) + " trăm")) + v + ((Dvi == 5 && Chuc > 0) ? " năm" : ((Dvi == 0L) ? "" : (" " + NumberToString(Dvi))));
		}


		private static bool connect()
		{
			string str = DBUtils.GetDBConnectionString();
			try
			{
				mySqlConnection = new SqlConnection(str);
				mySqlConnection.Open();
				return true;
			}
			catch (SqlException ex)
			{
				//MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
		}

		private static bool disconnect()
		{
			try
			{
				mySqlConnection.Close();
				return true;
			}
			catch (SqlException ex)
			{
				//MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
		}

		public static void ExecSP(string procedureName, params SqlParameter[] mySqlParameter)
		{
			try
			{
				if (connect())
				{
					SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
					mySqlCommand.CommandType = CommandType.StoredProcedure;
					for (int i = 0; i < mySqlParameter.Length; i++)
					{
						mySqlCommand.Parameters.Add(mySqlParameter[i]);
					}
					mySqlCommand.ExecuteNonQuery();
				}
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				disconnect();
			}
		}

		public static DataTable getTable(string procedureName, SqlParameter mySqlParameter, string nameSetToTable)
		{
			DataTable table = new DataTable();
			try
			{
				if (connect())
				{
					SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
					mySqlCommand.CommandType = CommandType.StoredProcedure;
					SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
					DataSet myDataSet = new DataSet();
					if (mySqlParameter != null)
					{
						mySqlCommand.Parameters.Add(mySqlParameter);
					}
					mySqlCommand.ExecuteNonQuery();
					mySqlDataAdapter.Fill(myDataSet, nameSetToTable);
					table = myDataSet.Tables[nameSetToTable];
				}
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				disconnect();
			}
			return table;
		}

		public static DataTable getTable(string procedureName, string nameSetToTable, params SqlParameter[] mySqlParameter)
		{
			DataTable table = new DataTable();
			try
			{
				if (connect())
				{
					SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
					mySqlCommand.CommandType = CommandType.StoredProcedure;
					SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
					DataSet myDataSet = new DataSet();
					for (int i = 0; i < mySqlParameter.Length; i++)
					{
						mySqlCommand.Parameters.Add(mySqlParameter[i]);
					}
					mySqlCommand.ExecuteNonQuery();
					mySqlDataAdapter.Fill(myDataSet, nameSetToTable);
					table = myDataSet.Tables[nameSetToTable];
				}
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				disconnect();
			}
			return table;
		}

		public static int SaveData(string procedureName, params SqlParameter[] mySqlParameter)
		{
			int result = 0;
			try
			{
				if (connect())
				{
					SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
					mySqlCommand.CommandType = CommandType.StoredProcedure;
					SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
					for (int i = 0; i < mySqlParameter.Length; i++)
					{
						mySqlCommand.Parameters.Add(mySqlParameter[i]);
					}
					mySqlCommand.ExecuteNonQuery();
					if (mySqlCommand.Parameters["@result"] != null)
					{
						result = Convert.ToInt16(mySqlCommand.Parameters["@result"].Value.ToString());
					}
				}
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				disconnect();
			}
			return result;
		}



		public static DataTable Select(string strComm)
		{
			SqlConnection cnn = new SqlConnection(DBUtils.GetDBConnectionString());
			SqlCommand cmd = new SqlCommand();
			SqlDataAdapter da = new SqlDataAdapter();
			try
			{
				cnn.Open();
				cmd = new SqlCommand("spSearchAllForTrans", cnn);
				cmd.CommandTimeout = 30;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", strComm));
				cmd.ExecuteNonQuery();
				da = new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();
				da.Fill(ds);
				return ds.Tables[0];
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cnn.Close();
			}
		}

		public static int Count(string strComm)
		{
			SqlConnection cnn = new SqlConnection(DBUtils.GetDBConnectionString());
			try
			{
				int count = 0;
				using (SqlCommand cmdCount = new SqlCommand(strComm, cnn))
				{
					cnn.Open();
					count = (int)cmdCount.ExecuteScalar();
				}
				return count;
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cnn.Close();
			}
		}

		public static bool CheckConnection(string strComm, string strConnectionString)
		{
			SqlConnection cnn = new SqlConnection(strConnectionString);
			SqlCommand cmd = new SqlCommand();
			SqlDataAdapter da = new SqlDataAdapter();
			try
			{
				cnn.Open();
				cmd = new SqlCommand("spSearchAllForTrans", cnn);
				cmd.CommandTimeout = 3;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", strComm));
				cmd.ExecuteNonQuery();
				da = new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();
				da.Fill(ds);
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					return false;
				}
			}
			finally
			{
				cnn.Close();
			}
			return true;
		}

		public static DataTable Select(string strComm, int _timeout)
		{
			SqlConnection cnn = new SqlConnection(DBUtils.GetDBConnectionString(_timeout));
			try
			{
				SqlCommand cmd = new SqlCommand();
				SqlDataAdapter da = new SqlDataAdapter();
				cnn.Open();
				cmd = new SqlCommand("spSearchAllForTrans", cnn);
				cmd.CommandTimeout = _timeout;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", strComm));
				cmd.ExecuteNonQuery();
				da = new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();
				da.Fill(ds);
				return ds.Tables[0];
			}
			catch (SqlException se)
			{
				if (se.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cnn.Close();
			}
		}

		public static string[] GetProfileName(string FullName)
		{
			string[] Result = new string[3];
			try
			{
				string[] Temp1 = FullName.Split(',');
				Result[2] = Temp1[0];
				if (Temp1.Length > 1)
				{
					Temp1[1] = Temp1[1].TrimStart(' ');
					string[] Temp2 = Temp1[1].Split(' ');
					if (Temp2.Length == 1)
					{
						Result[0] = Temp2[0];
						Result[1] = "";
					}
					else
					{
						Result[0] = Temp2[0];
						for (int i = 0; i < Temp2.Length; i++)
						{
							switch (i)
							{
								case 1:
									Result[1] += Temp2[i];
									break;
								default:
									Result[1] = Result[1] + " " + Temp2[i];
									break;
								case 0:
									break;
							}
						}
					}
				}
				else
				{
					Result[0] = (Result[1] = "");
				}
			}
			catch
			{
			}
			return Result;
		}

		public static decimal ExchangeCurrency(DateTime date, string FromCurrencyID, string ToCurrencyID, decimal Amount)
		{
			try
			{
				return Convert.ToDecimal(getTable("spExchangeCurrency", "Table", new SqlParameter("@DateTime", date), new SqlParameter("@FromCurrency", FromCurrencyID), new SqlParameter("@ToCurrency", ToCurrencyID), new SqlParameter("@Amount", Amount)).Rows[0][0].ToString());
			}
			catch
			{
				return 0m;
			}
		}

		public static DateTime GetBusinessDate()
		{
			try
			{
				return Convert.ToDateTime(getTable("spGetDate", null, "Table").Rows[0][0]);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static DateTime GetSystemDate()
		{
			try
			{
				return Convert.ToDateTime(getTable("spGetDateSystem", null, "Table").Rows[0][0]);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static DateTime GetBusinessDateTime()
		{
			try
			{
				DateTime B_Date = GetBusinessDate();
				DateTime S_Date = GetSystemDate();
				return new DateTime(B_Date.Year, B_Date.Month, B_Date.Day, S_Date.Hour, S_Date.Minute, S_Date.Second);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static bool CheckDelete(int IdDelete, string[] Field, string[] Table)
		{
			int count = 0;
			string[] paramName = new string[1] { "@sqlCommand" };
			string[] paramValue = new string[1];
			try
			{
				for (int i = 0; i < Field.Length; i++)
				{
					paramValue[0] = "select ID from " + Table[i] + " where Convert(nvarchar," + Field[i] + ")='" + IdDelete + "'";
					EventsLogErrorBO instance = EventsLogErrorBO.Instance;
					object[] paramValue2 = paramValue;
					if (instance.LoadDataFromSP("spGenSearchWithCommand", "Table", paramName, paramValue2).Rows.Count != 0)
					{
						count++;
					}
				}
				if (count != 0)
				{
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static bool CheckFieldNo(string Table, string Field, string ValueCheck)
		{
			string[] paramName = new string[1] { "@sqlCommand" };
			string[] paramValue = new string[1] { "select " + Field + " from [" + Table + "] where Convert(nvarchar," + Field + ")='" + ValueCheck + "'" };
			try
			{
				EventsLogErrorBO instance = EventsLogErrorBO.Instance;
				object[] paramValue2 = paramValue;
				if (instance.LoadDataFromSP("spGenSearchWithCommand", "Table", paramName, paramValue2).Rows.Count != 0)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		public static string GetXmlContent(BaseModel model)
		{
			try
			{
				string tableName = model.GetType().Name;
				tableName = tableName.Substring(0, tableName.Length - 5);
				string XMLWriter = "<?xml version='1.0'?>";
				PropertyInfo[] propertiesName = model.GetType().GetProperties();
				XMLWriter = XMLWriter + "<" + tableName + ">";
				for (int i = 0; i < propertiesName.Length; i++)
				{
					XMLWriter = XMLWriter + "<" + propertiesName[i].Name + ">";
					if (propertiesName[i].GetValue(model, null) != null)
					{
						XMLWriter = ((!(propertiesName[i].GetType() == typeof(DateTime))) ? (XMLWriter + propertiesName[i].GetValue(model, null).ToString()) : (XMLWriter + Convert.ToDateTime(propertiesName[i].GetValue(model, null)).ToString("yyyy/MM/dd hh:mm:ss")));
					}
					XMLWriter = XMLWriter + "</" + propertiesName[i].Name + ">";
				}
				return XMLWriter + "</" + tableName + ">";
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static BaseModel GetModelFromXML(string TableName, string XmlContent)
		{
			try
			{
				string fullname = "EzTicket." + ProcessTransaction.getClassName(TableName);
				BaseModel mB = (BaseModel)PropertyUtils.getObject(fullname);
				XmlTextReader Reader = new XmlTextReader(new StringReader(XmlContent));
				string[] paramName = new string[mB.GetType().GetProperties().Length];
				string[] paramValue = new string[mB.GetType().GetProperties().Length];
				int k = 0;
				while (Reader.Read())
				{
					if (Reader.NodeType == XmlNodeType.Element && !Reader.LocalName.Equals(TableName))
					{
						paramName[k] = Reader.LocalName;
						paramValue[k] = Reader.ReadString();
						k++;
					}
				}
				PropertyInfo[] propertiesName = mB.GetType().GetProperties();
				for (int i = 0; i < propertiesName.Length; i++)
				{
					for (int j = 0; j < paramName.Length; j++)
					{
						if (!propertiesName[i].Name.Equals(paramName[j]))
						{
							continue;
						}
						if (propertiesName[i].PropertyType.Name.Equals("Int32"))
						{
							propertiesName[i].SetValue(mB, Convert.ToInt32(paramValue[j]), null);
						}
						else if (propertiesName[i].PropertyType.Name.Equals("Decimal"))
						{
							propertiesName[i].SetValue(mB, Convert.ToDecimal(paramValue[j]), null);
						}
						else if (propertiesName[i].PropertyType.Name.Equals("DateTime"))
						{
							try
							{
								propertiesName[i].SetValue(mB, Convert.ToDateTime(paramValue[j]), null);
							}
							catch
							{
								propertiesName[i].SetValue(mB, DateTime.Now, null);
							}
						}
						else if (propertiesName[i].PropertyType.Name.Equals("Boolean"))
						{
							propertiesName[i].SetValue(mB, Convert.ToBoolean(paramValue[j]), null);
						}
						else if (propertiesName[i].PropertyType.Name.Equals("String"))
						{
							propertiesName[i].SetValue(mB, Convert.ToString(paramValue[j]), null);
						}
					}
				}
				return mB;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static string GetHostName()
		{
			return Dns.GetHostName();
		}

		public static void PopupColumnNameFromModel(BaseModel mB, ref DataTable Source)
		{
			try
			{
				DataTable dt = new DataTable();
				PropertyInfo[] propertiesName = mB.GetType().GetProperties();
				for (int i = 0; i < propertiesName.Length; i++)
				{
					dt.Columns.Add(propertiesName[i].Name, propertiesName[i].PropertyType);
				}
				Source = dt;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static DataTable PopupDataFromModel(BaseModel[] mB)
		{
			try
			{
				DataTable Source = new DataTable();
				PopupColumnNameFromModel(mB[0], ref Source);
				for (int i = 0; i < mB.Length; i++)
				{
					DataRow dr = Source.NewRow();
					PropertyInfo[] propertiesName = mB[i].GetType().GetProperties();
					for (int j = 0; j < propertiesName.Length; j++)
					{
						dr[j] = propertiesName[j].GetValue(mB[i], null);
					}
					Source.Rows.Add(dr);
				}
				return Source;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static void InsertTemp(string TableName, DataTable Source, int Index)
		{
			SqlConnection conn = new SqlConnection(DBUtils.GetDBConnectionString());
			string sql = DBUtils.SQLInsertTemp(TableName, Source);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandTimeout = 6000;
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			for (int i = 0; i < Source.Columns.Count; i++)
			{
				if (Source.Rows[Index][i] != null)
				{
					cmd.Parameters.Add(new SqlParameter("@" + Source.Columns[i].ColumnName, Source.Rows[Index][i]));
				}
				else
				{
					cmd.Parameters.Add(new SqlParameter("@" + Source.Columns[i].ColumnName, ""));
				}
			}
			try
			{
				conn.Open();
				cmd.ExecuteScalar();
			}
			catch (SqlException e)
			{
				throw new FacadeException(e.Message);
			}
			finally
			{
				conn.Close();
			}
		}





	

		public static int CompareDate(DateTime date1, DateTime date2)
		{
			if (date1.Day == date2.Day && date1.Month == date2.Month && date1.Year == date2.Year)
			{
				return 0;
			}
			if (date1.Year < date2.Year || (date1.Year == date2.Year && date1.Month < date2.Month) || (date1.Year == date2.Year && date1.Month == date2.Month && date1.Day < date2.Day))
			{
				return -1;
			}
			return 1;
		}

		public static string[] GetArrayTransaction(string strRouting)
		{
			string strReturn = "";
			string[] array = strRouting.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].Trim().Equals(""))
				{
					DataTable tb = Select("Select * from RoutingCode Where Code =N'" + array[i].ToString().Trim() + "'");
					strReturn = ((tb.Rows.Count <= 0) ? (strReturn + array[i].Trim() + ",") : (strReturn + tb.Rows[0]["TransactionCodes"].ToString().Trim() + ","));
				}
			}
			return strReturn.Split(',');
		}

		public static string FormatVND(decimal amount)
		{
			if (amount == 0m)
			{
				return "0";
			}
			return amount.ToString("###,###,###,###");
		}

		public static string GenerateCodeSequence(string table_name, string filed_name, string prefix, int sufix_lenght)
		{
			string sql = "Select * from " + table_name;
			string sufix = "";
			for (int i = 0; i < sufix_lenght; i++)
			{
				sufix += "0";
			}
			DataTable dt = Select(sql);
			if (dt.Rows.Count == 0)
			{
				return prefix + sufix;
			}
			string LastCode = dt.Rows[0][filed_name].ToString();
			int Temp = Convert.ToInt32(LastCode.Substring(LastCode.Length - 10, 10)) + 1;
			return prefix + sufix.Substring(0, 10 - Temp.ToString().Length) + Temp;
		}

		public static string GenerateCodeRandom(string tableName, string filed_name, string prefix, int sufix_lenght)
		{
			Random rnd = new Random();
			string sufix_min = "";
			for (int j = 0; j < sufix_lenght; j++)
			{
				sufix_min += "0";
			}
			string sufix_max = "";
			for (int i = 0; i < sufix_lenght; i++)
			{
				sufix_max += "9";
			}
			long max = Convert.ToInt64("99" + sufix_max);
			long min = Convert.ToInt64("99" + sufix_min);
			long code;
			do
			{
				code = Convert.ToInt64(((double)max * 1.0 - (double)min * 1.0) * rnd.NextDouble() + (double)min * 1.0);
			}
			while (Select("select * from " + tableName + " where " + filed_name + "='" + prefix + code + "'").Rows.Count > 0);
			string strCode = code.ToString();
			return prefix + strCode;
		}


		public void StartProcess(string path)
		{
			Process process = new Process();
			try
			{
				process.StartInfo.FileName = path;
				process.Start();
				process.WaitForInputIdle();
			}
			catch
			{
			}
		}


		public static bool CheckCashier()
		{
			return true;
		}

		public static bool CashierLogin(string UserName, string Password, ref int UserID, ref int SessionID, ref string Err)
		{
			try
			{
				Expression exp = new Expression("LoginName", UserName, "=");
				exp = exp.And(new Expression("PassWordHash", DBUtils.Encrypt(Password, DBUtils.passPhrase, DBUtils.saltValue, DBUtils.hashAlgorithm, DBUtils.passwordIterations, DBUtils.initVector, DBUtils.keySize), "="));
				exp = exp.And(new Expression("IsCashier", 1, "="));
				ArrayList arr = UsersBO.Instance.FindByExpression(exp);
				if (arr.Count == 0)
				{
					return false;
				}
				if (GetCashierShift(((UsersModel)arr[0]).ID, UserName, ref SessionID, ref Err))
				{
					UserID = ((UsersModel)arr[0]).ID;
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				Err = ex.Message;
				return false;
			}
		}

		public static bool GetCashierShift(int UserID, string UserName, ref int SessionID, ref string Err)
		{
			try
			{
				Expression exp = new Expression("UserID", UserID, "=");
				exp = exp.And(new Expression("Status", 0, "="));
				ArrayList arr = SessionBO.Instance.FindByExpression(exp);
				if (arr.Count == 0)
				{
					SessionModel mS = new SessionModel();
					mS.UserID = UserID;
					mS.StartTime = GetSystemDate();
					mS.EndTime = mS.StartTime;
					mS.Status = 0;
					SessionID = mS.ID;
					return true;
				}
				SessionID = ((SessionModel)arr[0]).ID;
				return true;
			}
			catch (Exception ex)
			{
				Err = ex.Message;
				return false;
			}
		}

		public static void ExcuteSQL(string strSQL)
		{
			SqlConnection cn = new SqlConnection(DBUtils.GetDBConnectionString());
			SqlCommand cmd = new SqlCommand(strSQL, cn);
			cmd.CommandType = CommandType.Text;
			cmd.CommandTimeout = 0;
			cn.Open();
			cmd.CommandText = strSQL;
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		public static string GenerateInvoiceCode(ProcessTransaction pt)
		{
			string sysDt = "";
			sysDt = ((pt != null) ? pt.GetSystemDate().ToString("ddMMyy") : GetSystemDate().ToString("ddMMyy"));
			string no = "0001";
			DataTable tbl = null;
			tbl = ((pt != null) ? pt.Select("SELECT TOP 1 InvoiceCode FROM Invoice WITH(NOLOCK) WHERE InvoiceCode LIKE '" + sysDt + "%' ORDER BY ID DESC") : Select("SELECT TOP 1 InvoiceCode FROM Invoice WITH(NOLOCK) WHERE InvoiceCode LIKE '" + sysDt + "%' ORDER BY ID DESC"));
			if (tbl.Rows.Count > 0)
			{
				no = ToString(ToInt(tbl.Rows[0][0].ToString().Substring(6)) + 1);
				while (no.Length < 4)
				{
					no = "0" + no;
				}
			}
			return sysDt + no;
		}



		public static void DeleteTmp()
		{
			try
			{
				string path = Path.GetTempPath();
				if (!(path != string.Empty))
				{
					return;
				}
				int count = 0;
				if (path.Substring(path.Length - 1, 1) != "\\")
				{
					path += "\\";
				}
				DirectoryInfo dir = new DirectoryInfo(path);
				if (!dir.Exists)
				{
					return;
				}
				FileInfo[] filesInDir = dir.GetFiles();
				FileInfo[] array = filesInDir;
				foreach (FileInfo file in array)
				{
					if (file.Extension == ".rpt")
					{
						string strPath = path + file.Name;
						if (!FileIsLocked(strPath))
						{
							count++;
							file.Delete();
						}
					}
				}
			}
			catch
			{
			}
		}

		public static bool FileIsLocked(string strFullFileName)
		{
			bool blnReturn = false;
			try
			{
				FileStream fs = File.Open(strFullFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
				fs.Close();
			}
			catch
			{
				blnReturn = true;
			}
			return blnReturn;
		}

		public static string GetSplitString(string Name)
		{
			string[] arrConfirmationNo = null;
			string paraName = "";
			if (Name != "")
			{
				arrConfirmationNo = Name.Split(',');
				if (arrConfirmationNo.Length != 0)
				{
					for (int i = 0; i < arrConfirmationNo.Length; i++)
					{
						paraName = ((i == 0) ? arrConfirmationNo[i].ToString().Trim() : (paraName + "','" + arrConfirmationNo[i].ToString().Trim()));
					}
				}
			}
			return paraName;
		}

		public static int _GetServiceByComputer(int _ComID)
		{
			int _SrvID = 0;
			DataTable dt = Select(("SELECT ServiceID FROM dbo.Computer WITH (NOLOCK) WHERE ID =" + _ComID) ?? "");
			if (dt != null && dt.Rows.Count > 0)
			{
				_SrvID = ToInt(dt.Rows[0]["ServiceID"]);
			}
			return _SrvID;
		}

		public static string Calculate_Char(int _Leng)
		{
			string _char = "";
			for (int i = 0; i < _Leng; i++)
			{
				_char = ((i != 0) ? (_char + "A") : "A");
			}
			return _char;
		}

		public static bool _IsAdministrator()
		{
			bool _isa = false;
			if (Global.AppUserName == "datdp")
			{
				_isa = true;
			}
			return _isa;
		}


		public static void ProcessCard()
		{
			DataTable Source = Select("SELECT * FROM dbo.Card WHERE ID IN ( SELECT DISTINCT CardID FROM dbo.Account WITH (NOLOCK) WHERE CardID NOT IN ( SELECT a.CardID FROM dbo.Account a WITH (NOLOCK) WHERE a.Status IN(0,1) AND a.EmployeeID =0 AND a.Rsv_ID = 0 ) AND [Status] =3 AND DATEDIFF(DAY, ExpirationDate,GETDATE()) > 0 AND EmployeeID =0 AND Rsv_ID = 0 AND TotalMoney > 0 AND CardID IN (SELECT ID FROM dbo.Card WHERE CanSell = 0) AND dbo.Account.BookingID > 0 ) AND dbo.Card.CanSell =0 AND dbo.Card.CardTypeID <> 35");
			if (Source.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < Source.Rows.Count; i++)
			{
				string _CardID = Source.Rows[i]["ID"].ToString();
				DataTable dt = Select("SELECT ID, Status, CardID FROM dbo.Account WITH (NOLOCK) WHERE CardID ='" + _CardID + "' ORDER BY ID DESC");
				bool _Check = true;
				if (dt.Rows.Count <= 0)
				{
					continue;
				}
				for (int c = 0; c < dt.Rows.Count; c++)
				{
					if (ToInt(dt.Rows[c]["Status"]) == 0 || ToInt(dt.Rows[c]["Status"]) == 1)
					{
						_Check = false;
						break;
					}
				}
				if (_Check)
				{
					UpdateByCommand(("UPDATE dbo.Account SET [Status] = 0 WHERE ID  = " + ToInt(dt.Rows[0]["ID"])) ?? "");
				}
			}
		}



		public static bool CheckExits(string Table, string Field, int ValueCheck)
		{
			string[] paramName = new string[1] { "@sqlCommand" };
			string[] paramValue = new string[1];
			object[] objArray1 = new object[8] { "Select ", Field, " from ", Table, " with (nolock) where Convert(nvarchar,", Field, ")=", ValueCheck };
			paramValue[0] = string.Concat(objArray1);
			try
			{
				LicenseBO instance = LicenseBO.Instance;
				object[] paramValue2 = paramValue;
				if (instance.LoadDataFromSP("spSearchAllForTrans", "Table", paramName, paramValue2).Rows.Count > 0)
				{
					return true;
				}
				return false;
			}
			catch (Exception exception)
			{
				throw new Exception(exception.Message);
			}
		}
	}
}