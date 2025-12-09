using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Ticket.Business;

namespace Ticket.Utils
{
	public class DBUtils
	{
		public enum ActionType
		{
			DELETE,
			INSERT,
			UPDATE,
			ERROR
		}

		public static string passPhrase = "Pas5pr@se";

		public static string saltValue = "s@1tValue";

		public static string hashAlgorithm = "SHA1";

		public static int passwordIterations = 2;

		public static string initVector = "@CSS@CSS@CSS@CSS";

		public static int keySize = 256;

		public static string strConnectDB(string pServer, string pDatabase, string pUsername, string pPassword)
		{
			try
			{
				StringBuilder strConnectDB = new StringBuilder();
				strConnectDB.Append("Server=" + pServer + ";");
				strConnectDB.Append("database=" + pDatabase + ";");
				strConnectDB.Append("uid=" + pUsername + ";");
				strConnectDB.Append("pwd=" + pPassword + ";");
				strConnectDB.Append("Connection Timeout=30;");
				return strConnectDB.ToString();
			}
			catch
			{
				throw new FacadeException("Can not connect to DB");
			}
		}

		public static bool GetInforConnectionString(ref string[] GetInfor, ref string err)
		{
			try
			{
				string[] Infor = new string[4];
				//string strPath = Application.StartupPath.ToString() + "\\default.ini";
                string strPath = System.Web.Configuration.WebConfigurationManager.AppSettings["default"];
                FileStream file = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Read);
				StreamReader sr = new StreamReader(file);
				for (int i = 0; i < 4; i++)
				{
					Infor[i] = sr.ReadLine();
				}
				sr.Close();
				file.Close();
				GetInfor = Infor;
				return true;
			}
			catch (Exception ex)
			{
				err = ex.Message;
				return false;
			}
		}

		public static bool GetInforConnectionString(ref string[] GetInfor, ref string err, string filename)
		{
			try
			{
				string[] Infor = new string[4];
				string strPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + filename;
				FileStream file = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Read);
				StreamReader sr = new StreamReader(file);
				for (int i = 0; i < 4; i++)
				{
					Infor[i] = sr.ReadLine();
				}
				sr.Close();
				file.Close();
				GetInfor = Infor;
				return true;
			}
			catch (Exception ex)
			{
				err = ex.Message;
				return false;
			}
		}

		public static bool GetParamConnectionString(string[] Input, ref string[] Ouput, ref string err)
		{
			try
			{
				for (int i = 0; i < Input.Length; i++)
				{
					Ouput[i] = Decrypt(Input[i], passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
				}
				return true;
			}
			catch (Exception ex)
			{
				err = ex.Message;
				return false;
			}
		}

		public static string Encrypt(string plainText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
		{
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
			byte[] keyBytes = password.GetBytes(keySize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] cipherTextBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(cipherTextBytes);
		}

		public static string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
		{
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
			byte[] keyBytes = password.GetBytes(keySize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
			MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
		}

		public static string GetDBConnectionString()
		{
			try
			{
				if (Global.ConnectionString.Equals(""))
				{
					string[] InputInfor = new string[4];
					string[] OutputInfor = new string[4];
					string err = "";
					GetInforConnectionString(ref InputInfor, ref err);
					GetParamConnectionString(InputInfor, ref OutputInfor, ref err);
					string pServerName = OutputInfor[0];
					string pDatabaseName = OutputInfor[1];
					string pUsername = OutputInfor[2];
					string pPassword = OutputInfor[3];
					StringBuilder connectionString = new StringBuilder();
					connectionString.Append("Server=" + pServerName + ";");
					connectionString.Append("database=" + pDatabaseName + ";");
					connectionString.Append("uid=" + pUsername + ";");
					connectionString.Append("pwd= " + pPassword + ";");
					connectionString.Append("Connection Timeout= 30;");
					Global.ConnectionString = connectionString.ToString();
				}
				return Global.ConnectionString;
			}
			catch
			{
				throw new FacadeException("Can not connect to DB");
			}
		}

		public static string GetDBConnectionString(int _timeout)
		{
			try
			{
				if (Global.ConnectionString.Equals(""))
				{
					string[] InputInfor = new string[4];
					string[] OutputInfor = new string[4];
					string err = "";
					GetInforConnectionString(ref InputInfor, ref err);
					GetParamConnectionString(InputInfor, ref OutputInfor, ref err);
					string pServerName = OutputInfor[0];
					string pDatabaseName = OutputInfor[1];
					string pUsername = OutputInfor[2];
					string pPassword = OutputInfor[3];
					StringBuilder connectionString = new StringBuilder();
					connectionString.Append("Server=" + pServerName + ";");
					connectionString.Append("database=" + pDatabaseName + ";");
					connectionString.Append("uid=" + pUsername + ";");
					connectionString.Append("pwd= " + pPassword + ";");
					connectionString.Append("Connection Timeout= " + _timeout + ";");
					Global.ConnectionString = connectionString.ToString();
				}
				return Global.ConnectionString;
			}
			catch
			{
				throw new FacadeException("Can not connect to DB");
			}
		}

		public static string GetBookingDBConnectionString()
		{
			try
			{
				string filename = "default_PMS.ini";
				string[] InputInfor = new string[4];
				string[] OutputInfor = new string[4];
				string err = "";
				GetInforConnectionString(ref InputInfor, ref err, filename);
				GetParamConnectionString(InputInfor, ref OutputInfor, ref err);
				string pServerName = OutputInfor[0];
				string pDatabaseName = OutputInfor[1];
				string pUsername = OutputInfor[2];
				string pPassword = OutputInfor[3];
				StringBuilder connectionString = new StringBuilder();
				connectionString.Append("Server=" + pServerName + ";");
				connectionString.Append("database=" + pDatabaseName + ";");
				connectionString.Append("uid=" + pUsername + ";");
				connectionString.Append("pwd= " + pPassword + ";");
				connectionString.Append("Connection Timeout=10;");
				return connectionString.ToString();
			}
			catch
			{
				throw new FacadeException("Can not connect to DB");
			}
		}

		public static string GetFastDBConnectionString()
		{
			try
			{
				string filename = "default_Fast.ini";
				string[] InputInfor = new string[4];
				string[] OutputInfor = new string[4];
				string err = "";
				GetInforConnectionString(ref InputInfor, ref err, filename);
				GetParamConnectionString(InputInfor, ref OutputInfor, ref err);
				string pServerName = OutputInfor[0];
				string pDatabaseName = OutputInfor[1];
				string pUsername = OutputInfor[2];
				string pPassword = OutputInfor[3];
				StringBuilder connectionString = new StringBuilder();
				connectionString.Append("Server=" + pServerName + ";");
				connectionString.Append("database=" + pDatabaseName + ";");
				connectionString.Append("uid=" + pUsername + ";");
				connectionString.Append("pwd= " + pPassword + ";");
				connectionString.Append("Connection Timeout=10;");
				return connectionString.ToString();
			}
			catch
			{
				throw new FacadeException("Can not connect to DB");
			}
		}

		public static string GetImageDBConnectionString()
		{
			try
			{
				string filename = "default_image.ini";
				string[] InputInfor = new string[4];
				string[] OutputInfor = new string[4];
				string err = "";
				GetInforConnectionString(ref InputInfor, ref err, filename);
				GetParamConnectionString(InputInfor, ref OutputInfor, ref err);
				string pServerName = OutputInfor[0];
				string pDatabaseName = OutputInfor[1];
				string pUsername = OutputInfor[2];
				string pPassword = OutputInfor[3];
				StringBuilder connectionString = new StringBuilder();
				connectionString.Append("Server=" + pServerName + ";");
				connectionString.Append("database=" + pDatabaseName + ";");
				connectionString.Append("uid=" + pUsername + ";");
				connectionString.Append("pwd= " + pPassword + ";");
				connectionString.Append("Connection Timeout=10;");
				return connectionString.ToString();
			}
			catch
			{
				throw new FacadeException("Can not connect to DB");
			}
		}

		public static string GetReportConnectionString()
		{
			string pServerName = "";
			string pDatabaseName = "";
			string pUsername = "";
			string pPassword = "";
			string strPath = AppDomain.CurrentDomain.BaseDirectory + "\\default.ini";
			//string strPath = System.Web.Configuration.WebConfigurationManager.AppSettings["default"];
			FileStream file = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Read);
			StreamReader sr = new StreamReader(file);
			for (int i = 0; i < 4; i++)
			{
				if (i == 0)
				{
					pServerName = sr.ReadLine();
				}
				if (i == 1)
				{
					pDatabaseName = sr.ReadLine();
				}
				if (i == 2)
				{
					pUsername = sr.ReadLine();
				}
				if (i == 3)
				{
					pPassword = sr.ReadLine();
				}
			}
			sr.Close();
			file.Close();
			try
			{
				StringBuilder connectionString = new StringBuilder();
				connectionString.Append("Provider=SQLOLEDB.1;");
				connectionString.Append("Server=" + pServerName.Trim() + ";");
				connectionString.Append("database=" + pDatabaseName.Trim() + ";");
				connectionString.Append("uid=" + pUsername + ";");
				connectionString.Append("pwd= " + pPassword + ";");
				connectionString.Append("Connection Timeout=100;");
				return connectionString.ToString();
			}
			catch
			{
				throw new FacadeException("Can not connect to DB");
			}
		}

		public static string SQLInsert(BaseModel mode)
		{
			string tableName = mode.GetType().Name;
			tableName = tableName.Substring(0, tableName.Length - 5);
			string Insert = "insert into " + tableName + " (";
			PropertyInfo[] propertiesName = mode.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				if (!tableName.Equals("Tree_Members") && !tableName.Equals("PaymentTrans"))
				{
					if (!propertiesName[i].Name.Equals("ID"))
					{
						Insert += propertiesName[i].Name;
						Insert += ",";
					}
				}
				else
				{
					Insert += propertiesName[i].Name;
					Insert += ",";
				}
			}
			Insert = Insert.Substring(0, Insert.Length - 1);
			Insert += ") values (";
			for (int j = 0; j < propertiesName.Length; j++)
			{
				if (!tableName.Equals("Tree_Members") && !tableName.Equals("PaymentTrans"))
				{
					if (!propertiesName[j].Name.Equals("ID"))
					{
						Insert += "@";
						Insert += propertiesName[j].Name;
						Insert += ",";
					}
				}
				else
				{
					Insert += "@";
					Insert += propertiesName[j].Name;
					Insert += ",";
				}
			}
			Insert = Insert.Substring(0, Insert.Length - 1);
			if (!tableName.Equals("Tree_Members") && !tableName.Equals("PaymentTrans"))
			{
				return Insert + ") SELECT @@IDENTITY AS 'ID'";
			}
			return Insert + ")";
		}

		public static string SQLInsertTemp(string tableName, DataTable Source)
		{
			string Insert = "insert into " + tableName + " (";
			for (int j = 0; j < Source.Columns.Count; j++)
			{
				Insert += Source.Columns[j].ColumnName;
				Insert += ",";
			}
			Insert = Insert.Substring(0, Insert.Length - 1);
			Insert += ") values (";
			for (int i = 0; i < Source.Columns.Count; i++)
			{
				Insert += "@";
				Insert += Source.Columns[i].ColumnName;
				Insert += ",";
			}
			Insert = Insert.Substring(0, Insert.Length - 1);
			return Insert + ")";
		}

		public static string SQLUpdate(BaseModel model)
		{
			string tableName = model.GetType().Name;
			tableName = tableName.Substring(0, tableName.Length - 5);
			string Update = "UPDATE " + tableName + " SET ";
			PropertyInfo[] propertiesName = model.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				if (!propertiesName[i].Name.Equals("ID"))
				{
					Update += propertiesName[i].Name;
					Update = Update + "=@" + propertiesName[i].Name;
					Update += ",";
				}
			}
			Update = Update.Substring(0, Update.Length - 1);
			if (ConvertToSQLType(propertiesName[0].PropertyType) == SqlDbType.NVarChar)
			{
				return Update + " WHERE ID='" + model.GetType().GetProperty("ID").GetValue(model, null)
					.ToString() + "'";
			}
			return Update + " WHERE ID=" + model.GetType().GetProperty("ID").GetValue(model, null)
				.ToString();
		}

		public static string SQLUpdate(BaseModel model, string field)
		{
			string tableName = model.GetType().Name;
			tableName = tableName.Substring(0, tableName.Length - 5);
			string Update = "UPDATE " + tableName + " SET ";
			PropertyInfo[] propertiesName = model.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				if (!propertiesName[i].Name.Equals("ID") && !propertiesName[i].Name.Equals(field))
				{
					Update += propertiesName[i].Name;
					Update = Update + "=@" + propertiesName[i].Name;
					Update += ",";
				}
			}
			Update = Update.Substring(0, Update.Length - 1);
			return Update + " WHERE ID=" + model.GetType().GetProperty("ID").GetValue(model, null)
				.ToString();
		}

		public static string SQLSelect(string tableName, string field, object value)
		{
			return SQLSelect(tableName, new Expression(field, value));
		}

		public static string SQLSelect(string tableName, string field, object value, string op)
		{
			return SQLSelect(tableName, new Expression(field, value, op));
		}

		public static string SQLSelect(string tableName, Expression exp)
		{
			string sql = "SELECT * FROM " + tableName + " WITH (NOLOCK) ";
			if (exp != null)
			{
				sql = sql + " WHERE " + exp;
			}
			return sql;
		}

		public static string SQLSelectMax(string tableName, string field, string field1, object value)
		{
			return SQLSelectMax(tableName, field, new Expression(field1, value));
		}

		public static string SQLSelectMaxRoot(string tableName, string field)
		{
			return SQLSelectMax(tableName, field);
		}

		public static string SQLSelectMax(string tableName, string field)
		{
			return "SELECT Max(" + field + ") FROM " + tableName + " WITH (NOLOCK) ";
		}

		public static string SQLSelectMinRoot(string tableName, string field)
		{
			return SQLSelectMin(tableName, field);
		}

		public static string SQLSelectMin(string tableName, string field)
		{
			return "SELECT Min(" + field + ") FROM " + tableName + " WITH (NOLOCK) ";
		}

		public static string SQLSelectMax(string tableName, string field, Expression exp)
		{
			string sql = "SELECT Max(" + field + ") FROM " + tableName + " WITH (NOLOCK) ";
			if (exp != null)
			{
				sql = sql + " WHERE " + exp;
			}
			return sql;
		}

		public static string SQLSelectCount(string tableName, string field, Expression exp)
		{
			string sql = "SELECT count(" + field + ") FROM " + tableName + " WITH (NOLOCK) ";
			if (exp != null)
			{
				sql = sql + " WHERE " + exp;
			}
			return sql;
		}

		public static string SQLSelectCount(string tableName, string field, string exp)
		{
			string sql = "SELECT count(" + field + ") FROM " + tableName + " WITH (NOLOCK) ";
			if (exp != null)
			{
				sql = sql + " WHERE " + exp;
			}
			return sql;
		}

		public static string SQLDelete(string tableName, Expression exp)
		{
			string sql = "DELETE FROM " + tableName;
			if (exp != null)
			{
				sql = sql + " WHERE " + exp;
			}
			return sql;
		}

		public static string SQLDelete(string tableName, long IDvalue)
		{
			return "DELETE FROM " + tableName + " WHERE ID=" + IDvalue;
		}

		public static string SQLDelete(string tableName, string field, string value)
		{
			return "delete FROM " + tableName + " WHERE " + field + "='" + value + "'";
		}

		public static string SQLDelete(string tableName, string field, long value)
		{
			return "delete FROM " + tableName + " WHERE " + field + "=" + value;
		}

		public static SqlDbType ConvertToSQLType(Type type)
		{
			if (type == typeof(string))
			{
				return SqlDbType.NVarChar;
			}
			if (type == typeof(int))
			{
				return SqlDbType.Int;
			}
			if (type == typeof(short))
			{
				return SqlDbType.TinyInt;
			}
			if (type == typeof(long))
			{
				return SqlDbType.BigInt;
			}
			if (type == typeof(DateTime))
			{
				return SqlDbType.DateTime;
			}
			if (type == typeof(long))
			{
				return SqlDbType.BigInt;
			}
			if (type == typeof(decimal))
			{
				return SqlDbType.Decimal;
			}
			if (type == typeof(byte[]))
			{
				return SqlDbType.Image;
			}
			if (type == typeof(Guid))
			{
				return SqlDbType.UniqueIdentifier;
			}
			if (type == typeof(double))
			{
				return SqlDbType.Float;
			}
			return SqlDbType.NVarChar;
		}

		private static string GetDescription(string strTableName, ActionType Action)
		{
			return string.Concat("Table: ", strTableName, " Action: ", Action, " ");
		}

		public static string GetDescription(string strTableName, ActionType Action, string Code)
		{
			return GetDescription(strTableName, Action) + "With Code: " + Code + " Event: OK";
		}

		public static string GetDescription(string strTableName, ActionType Action, int ID)
		{
			return GetDescription(strTableName, Action) + "With iD: " + ID + " Event: OK";
		}

		public static string GetDescription(string strTableName, ActionType Action, string Code, string strError)
		{
			return GetDescription(strTableName, Action) + "With Code: " + Code + " Event: " + strError;
		}

		public static string GetDescription(string strTableName, ActionType Action, int ID, string strError)
		{
			return GetDescription(strTableName, Action) + "With iD: " + ID + " Event: " + strError;
		}

		public static string GetDescription(string strTableName, ActionType Action, string strColumnName, int value)
		{
			return GetDescription(strTableName, Action) + "With " + strColumnName + ": " + value + " Event: OK";
		}

		public static string GetDescription(string strTableName, ActionType Action, string strColumnName, int value, string strError)
		{
			return GetDescription(strTableName, Action) + "With " + strColumnName + ": " + value + " Event: " + strError;
		}

		public static string GetDescription(string strTableName, ActionType Action, string strColumnName, string value, string strError)
		{
			return GetDescription(strTableName, Action) + "With " + strColumnName + ": " + value + " Event: " + strError;
		}

		public static string GetSystemInfo(string keyName, string defaultValue = null, string description = null, string dbConnectionString = null)
		{
			SystemInfoModel config = new SystemInfoModel();
			ArrayList arr = SystemInfoBO.Instance.FindByAttribute("KeyName", keyName);
			if (arr.Count == 0 && defaultValue != null)
			{
				config.KeyName = keyName;
				config.KeyValue = defaultValue;
				config.Description = description ?? "";
				config.CreatedBy = "auto";
				config.UpdatedBy = "auto";
				config.CreatedDate = SystemInfoBO.Instance.GetSystemDate();
				config.UpdatedDate = config.CreatedDate;
				SystemInfoBO.Instance.Insert(config);
			}
			else if (arr.Count > 0)
			{
				config = (SystemInfoModel)arr[0];
			}
			return config.KeyValue;
		}
	}
}
