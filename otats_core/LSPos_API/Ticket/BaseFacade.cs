using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using Ticket.Utils;

namespace Ticket
{
	public class BaseFacade
	{
		public static string strcon = Global.ConnectionString;

		public const string strNetworkErrorMgs = "Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp";

		private string className;

		private string tableName;

		private EventsLogModel mEL;

		protected BaseModel baseModel = new BaseModel();

		public string DataTableName
		{
			get
			{
				string className = GetType().Name;
				return className.Substring(0, className.Length - 6);
			}
		}

		protected BaseFacade()
		{
		}

		public string Audit(BaseModel obj)
		{
			string Changes = "";
			if (obj != null)
			{
				string name = obj.GetType().Name;
				name.Substring(0, name.Length - 5);
				long id = long.Parse(PropertyUtils.GetValue(obj, name.Substring(0, name.Length - 5) + "iD").ToString());
				BaseModel oldObject = null;
				oldObject = FindByPK(id);
				Changes = obj.CompareTo(oldObject);
			}
			return Changes;
		}

		public string fordate(DateTime dt)
		{
			return DateTime.Parse(dt.ToShortDateString(), new CultureInfo("en-US", useUserOverride: true)).ToString("MM/dd/yyyy");
		}

		public string fordate(string dt)
		{
			return DateTime.Parse(dt, new CultureInfo("en-US", useUserOverride: true)).ToString("MM/dd/yyyy");
		}

		public BaseFacade(BaseModel baseModel)
		{
			this.baseModel = baseModel;
			className = GetType().Name;
			className = className.Substring(0, className.Length - 6) + "Model";
			tableName = className.Substring(0, className.Length - 5);
		}

		public virtual BaseModel FindByPK(long value)
		{
			return FindModel(string.Format("SELECT * FROM [{0}] WITH (NOLOCK) WHERE ID = {2}", tableName, tableName, value));
		}

		public virtual ArrayList FindByPK(ArrayList list)
		{
			return FindByPK(PropertyUtils.ToListWithComma(list));
		}

		public virtual ArrayList FindHierarchicallyByPK(int PK, string parentFieldName)
		{
			ArrayList arr = new ArrayList();
			arr.Add(FindByPK(PK));
			ArrayList chilrenArr = FindByAttr(parentFieldName, PK);
			if (chilrenArr.Count > 0)
			{
				foreach (BaseModel child in chilrenArr)
				{
					int childPK = Convert.ToInt32(child.GetType().GetProperty("iD").GetValue(child, null));
					arr.AddRange(FindHierarchicallyByPK(childPK, parentFieldName));
				}
			}
			return arr;
		}

		public virtual ArrayList FindByPK(string list)
		{
			return ExecuteSQL($"SELECT * FROM {tableName} WITH (NOLOCK) WHERE {tableName}iD IN ({list})");
		}

		public BaseModel FindByUK(string field, string value)
		{
			if (value.IndexOf('\'') >= 0)
			{
				value = value.Replace("'", "''");
			}
			return FindModel($"SELECT * FROM {tableName} WITH (NOLOCK) WHERE {field} = '{value}'");
		}

		public ArrayList FindAll()
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, null));
		}

		public ArrayList FindAllNoLimit()
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, null));
		}

		public ArrayList FindByAttr(string field, string value)
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, field, value));
		}

		public ArrayList FindByAttrWithOrder(string field, string value, string FieldOrder, string OrderBy)
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, field, value) + " " + baseModel.GetOrderBy(FieldOrder, OrderBy));
		}

		public ArrayList FindByAttr(string field, long value)
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, field, value));
		}

		public ArrayList FindByAttrWithOrder(string field, long value, string FieldOrder, string OrderBy)
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, field, value) + " " + baseModel.GetOrderBy(FieldOrder, OrderBy));
		}

		public ArrayList FindByExpression(Expression exp)
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, exp));
		}

		public ArrayList FindByExpressionWithOrder(Expression exp, string FieldOrder, string OrderBy)
		{
			return ExecuteSQL(DBUtils.SQLSelect(tableName, exp) + " " + baseModel.GetOrderBy(FieldOrder, OrderBy));
		}

		public Hashtable LazyLoad()
		{
			return LazyLoad(tableName + "Name");
		}

		public Hashtable LazyLoad(string name)
		{
			string Field_ID = "iD";
			string sql = $"SELECT {Field_ID} AS f1, {name} AS f2 FROM {tableName} WITH (NOLOCK)";
			return LazyLoadToHashtable(sql);
		}

		protected Hashtable LazyLoadToHashtable(string sql)
		{
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				Hashtable result = new Hashtable();
				while (reader.Read())
				{
					if (!result.Contains(reader["f1"]))
					{
						result.Add(reader["f1"], reader["f2"]);
					}
				}
				return result;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		protected BaseModel FindModel(string sql)
		{
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			SqlDataReader reader = null;
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				if (reader.Read())
				{
					return PropertyUtils.PopulateModel(reader, className);
				}
				return null;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		public virtual decimal Insert(BaseModel baseModel)
		{
			string TableName = baseModel.GetType().Name.Substring(0, baseModel.GetType().Name.Length - 5);
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			string sql = DBUtils.SQLInsert(baseModel);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			PropertyInfo[] propertiesName = baseModel.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				SqlDbType dbType = DBUtils.ConvertToSQLType(propertiesName[i].PropertyType);
				object value = propertiesName[i].GetValue(baseModel, null);
				if (!TableName.Equals("Tree_Members") && !TableName.Equals("PaymentTrans"))
				{
					if (!propertiesName[i].Name.Equals("ID") && !propertiesName[i].Name.Equals("iD"))
					{
						if (value != null)
						{
							cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = value;
						}
						else if (dbType == SqlDbType.Image)
						{
							cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = DBNull.Value;
						}
						else
						{
							cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = "";
						}
					}
				}
				else if (value != null)
				{
					cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = value;
				}
				else if (dbType == SqlDbType.Image)
				{
					cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = DBNull.Value;
				}
				else
				{
					cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = "";
				}
			}
			try
			{
				conn.Open();
				decimal id = (decimal)cmd.ExecuteScalar();
				cmd.Parameters.Clear();
				return id;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		protected virtual int UpdateField(ArrayList list, string field, int value)
		{
			if (list == null || list.Count == 0)
			{
				return 0;
			}
			string sql = string.Format("UPDATE {0} SET {1} = {2} WHERE {0}iD IN ({3})", tableName, field, value, PropertyUtils.ToListWithComma(list));
			return ExecuteNonQuerySQL(sql);
		}

		public virtual int Update(BaseModel baseModel, string field)
		{
			string TableName = baseModel.GetType().Name.Substring(0, baseModel.GetType().Name.Length - 5);
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			string sql = DBUtils.SQLUpdate(baseModel, field);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandTimeout = 6000;
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			PropertyInfo[] propertiesName = baseModel.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				SqlDbType dbType = DBUtils.ConvertToSQLType(propertiesName[i].PropertyType);
				object value = propertiesName[i].GetValue(baseModel, null);
				if (value != null)
				{
					if (propertiesName[i].PropertyType.Name.Equals("Byte[]"))
					{
						cmd.Parameters.Add("@" + propertiesName[i].Name, SqlDbType.Image).Value = value;
					}
					else
					{
						cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = value;
					}
				}
				else
				{
					cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = "";
				}
			}
			try
			{
				conn.Open();
				return cmd.ExecuteNonQuery();
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
			}
		}

		public virtual int Update(BaseModel baseModel)
		{
			string TableName = baseModel.GetType().Name.Substring(0, baseModel.GetType().Name.Length - 5);
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			string sql = DBUtils.SQLUpdate(baseModel);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			PropertyInfo[] propertiesName = baseModel.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				SqlDbType dbType = DBUtils.ConvertToSQLType(propertiesName[i].PropertyType);
				object value = propertiesName[i].GetValue(baseModel, null);
				if (value != null)
				{
					if (propertiesName[i].PropertyType.Name.Equals("Byte[]"))
					{
						cmd.Parameters.Add("@" + propertiesName[i].Name, SqlDbType.Image).Value = value;
					}
					else
					{
						cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = value;
					}
				}
				else if (dbType == SqlDbType.Image)
				{
					cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = DBNull.Value;
				}
				else
				{
					cmd.Parameters.Add("@" + propertiesName[i].Name, dbType).Value = "";
				}
			}
			try
			{
				conn.Open();
				int rs = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				return rs;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		protected int ExecuteNonQuerySQL(string sql)
		{
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = new SqlCommand(sql, conn);
			cmd.CommandType = CommandType.Text;
			try
			{
				cmd.Connection.Open();
				return cmd.ExecuteNonQuery();
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		protected int ExecuteNonQuerySQL(string sql, EventsLogModel mE)
		{
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = new SqlCommand(sql, conn);
			cmd.CommandType = CommandType.Text;
			cmd.CommandTimeout = 6000;
			try
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
				PropertyInfo[] propertiesName1 = mE.GetType().GetProperties();
				sql = DBUtils.SQLInsert(mE);
				for (int i = 0; i < propertiesName1.Length; i++)
				{
					object value = propertiesName1[i].GetValue(mE, null);
					if (!propertiesName1[i].Name.Equals("iD"))
					{
						if (value != null)
						{
							cmd.Parameters.Add("@" + propertiesName1[i].Name, DBUtils.ConvertToSQLType(propertiesName1[i].PropertyType)).Value = value;
						}
						else
						{
							cmd.Parameters.Add("@" + propertiesName1[i].Name, DBUtils.ConvertToSQLType(propertiesName1[i].PropertyType)).Value = "";
						}
					}
				}
				cmd.CommandText = sql;
				return cmd.ExecuteNonQuery();
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		public virtual void Delete(long IDValue)
		{
			string sql = DBUtils.SQLDelete(tableName, IDValue);
			try
			{
				ExecuteNonQuerySQL(sql);
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
		}

		public virtual void DeleteByExpression(Expression exp)
		{
			string sql = DBUtils.SQLDelete(tableName, exp);
			ExecuteNonQuerySQL(sql);
		}

		public virtual void Delete(ArrayList listID)
		{
			string sql = "DELETE FROM " + tableName + " WHERE iD IN (" + PropertyUtils.ToListWithComma(listID) + ")";
			ExecuteNonQuerySQL(sql);
		}

		public void DeleteByAttribute(string name, string value)
		{
			string sql = DBUtils.SQLDelete(tableName, name, value);
			ExecuteNonQuerySQL(sql);
		}

		public void DeleteByAttribute(string name, long value)
		{
			string sql = DBUtils.SQLDelete(tableName, name, value);
			ExecuteNonQuerySQL(sql);
		}

		public object FindByMax(string field, string field1, string value)
		{
			return ExecuteScalar(DBUtils.SQLSelectMax(tableName, field, field1, value));
		}

		public object FindByCount(string field, string expression)
		{
			return ExecuteScalar(DBUtils.SQLSelectCount(tableName, field, expression));
		}

		public object FindByMaxRoot(string field)
		{
			return ExecuteScalar(DBUtils.SQLSelectMaxRoot(tableName, field));
		}

		public object FindByMinRoot(string field)
		{
			return ExecuteScalar(DBUtils.SQLSelectMinRoot(tableName, field));
		}

		protected ArrayList ExecuteSQL(string sql)
		{
			SqlConnection conn = new SqlConnection(DBUtils.GetDBConnectionString());
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				ArrayList result = new ArrayList();
				while (reader.Read())
				{
					result.Add(PropertyUtils.PopulateModel(reader, className));
				}
				return result;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		protected ArrayList ExecuteSQLNotFW(string sql)
		{
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			DateTime time = DateTime.Now;
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				ArrayList result = new ArrayList();
				while (reader.Read())
				{
					result.Add(reader["column_name"]);
				}
				return result;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		protected void AssignNN(string table1, string table2, int id, ArrayList list)
		{
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			string sqlDelete = DBUtils.SQLDelete(table1 + table2, table1 + "iD", id);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sqlDelete;
			cmd.CommandType = CommandType.Text;
			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
			}
			catch (IndexOutOfRangeException iex2)
			{
				throw new Exception("Column \"" + iex2.Message + "\" does not exist in table \"" + tableName + "\"");
			}
			catch (SqlException se2)
			{
				if (se2.Class == 20)
				{
					throw new Exception("Không kết nối được tới Server ! Hãy liên hệ với bộ phận IT để được trợ giúp");
				}
				throw new Exception(se2.Message);
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
			string sqlInsert = "insert into " + table1 + table2 + " (" + table1 + "iD," + table2 + "iD) ";
			IEnumerator ie = list.GetEnumerator();
			while (ie.MoveNext())
			{
				cmd.CommandText = string.Concat(sqlInsert, " VALUES(", id, ", ", ie.Current, ")");
				cmd.CommandType = CommandType.Text;
				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (IndexOutOfRangeException iex)
				{
					throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
			}
			conn.Close();
		}

		public bool CheckExist(string field, string value)
		{
			string sql = string.Format("SELECT {0} FROM {1} WITH (NOLOCK) WHERE {0} = '{2}'", field, tableName, value);
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader();
				return reader.HasRows;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		public bool CheckExist(string field, long value)
		{
			string sql = string.Format("SELECT TOP 1 {0} FROM {1} WITH (NOLOCK) WHERE {0} = {2}", field, tableName, value);
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				return reader.HasRows;
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		public object SelectTOP(string field, string order)
		{
			string sql = string.Format("SELECT TOP 1 {0} AS T FROM {1} WITH (NOLOCK) ORDER BY {0} {2}", field, tableName, order);
			return ExecuteScalar(sql);
		}

		protected object ExecuteScalar(string sql)
		{
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			cmd.CommandTimeout = 6000;
			try
			{
				conn.Open();
				return cmd.ExecuteScalar();
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
		}

		public string GenerateNo(string code)
		{
			string sql = "SELECT TOP 1 " + code + " FROM " + tableName + " ORDER BY iD DESC";
			string lastBillNo = "";
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			ArrayList result = new ArrayList();
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				if (reader.Read())
				{
					lastBillNo = reader[0].ToString();
				}
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
			if (lastBillNo.Length == 0)
			{
				return "0001";
			}
			string digitPart = "";
			string stringPart = lastBillNo;
			int i = lastBillNo.Length - 1;
			while (i >= 0)
			{
				try
				{
					Convert.ToInt32(lastBillNo.Substring(i, 1));
					digitPart = lastBillNo.Substring(i, 1) + digitPart;
					i--;
				}
				catch
				{
					break;
				}
			}
			if (digitPart.Length > 0)
			{
				stringPart = lastBillNo.Substring(0, i + 1);
				string newDigitPart = Convert.ToString(Convert.ToInt32(digitPart) + 1);
				switch (newDigitPart.Length)
				{
				case 1:
					newDigitPart = "000" + newDigitPart;
					break;
				case 2:
					newDigitPart = "00" + newDigitPart;
					break;
				case 3:
					newDigitPart = "0" + newDigitPart;
					break;
				}
				return stringPart + newDigitPart;
			}
			return lastBillNo + "0001";
		}

		public string GenerateNo1(string code, int ClassID)
		{
			string sql = "SELECT TOP 1 " + code + " FROM " + tableName + "  WHERE   ClassID = " + ClassID + " And Right(" + code + ",2)!='GV' ORDER BY " + tableName + "iD DESC";
			string lastBillNo = "";
			string lastBillNo2 = "";
			string sql2 = "";
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			ArrayList result = new ArrayList();
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				if (reader.Read())
				{
					lastBillNo = reader[0].ToString();
				}
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
			if (lastBillNo.Length == 0)
			{
				return lastBillNo2 + "0001";
			}
			string digitPart = "";
			string stringPart = lastBillNo;
			int i = lastBillNo.Length - 1;
			while (i >= 0)
			{
				try
				{
					Convert.ToInt32(lastBillNo.Substring(i, 1));
					digitPart = lastBillNo.Substring(i, 1) + digitPart;
					i--;
				}
				catch
				{
					break;
				}
			}
			if (digitPart.Length > 0)
			{
				stringPart = lastBillNo.Substring(0, i + 1);
				string newDigitPart = Convert.ToString(Convert.ToInt32(digitPart) + 1);
				switch (newDigitPart.Length)
				{
				case 1:
					newDigitPart = "000" + newDigitPart;
					break;
				case 2:
					newDigitPart = "00" + newDigitPart;
					break;
				case 3:
					newDigitPart = "0" + newDigitPart;
					break;
				}
				return stringPart + newDigitPart;
			}
			return lastBillNo + "0001";
		}

		public string GenerateNo2(string code, int ClassID)
		{
			string sql = "SELECT TOP 1 " + code + " FROM " + tableName + "  WHERE  parentID = " + ClassID + " ORDER BY iD DESC";
			string lastBillNo = "";
			SqlConnection conn = new SqlConnection(Global.ConnectionString);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			ArrayList result = new ArrayList();
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				if (reader.Read())
				{
					lastBillNo = reader[0].ToString();
				}
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
			if (lastBillNo.Length == 0)
			{
				return "0001";
			}
			string digitPart = "";
			string stringPart = lastBillNo;
			int i = lastBillNo.Length - 1;
			while (i >= 0)
			{
				try
				{
					Convert.ToInt32(lastBillNo.Substring(i, 1));
					digitPart = lastBillNo.Substring(i, 1) + digitPart;
					i--;
				}
				catch
				{
					break;
				}
			}
			if (digitPart.Length > 0)
			{
				stringPart = lastBillNo.Substring(0, i + 1);
				string newDigitPart = Convert.ToString(Convert.ToInt32(digitPart) + 1);
				switch (newDigitPart.Length)
				{
				case 1:
					newDigitPart = "000" + newDigitPart;
					break;
				case 2:
					newDigitPart = "00" + newDigitPart;
					break;
				case 3:
					newDigitPart = "0" + newDigitPart;
					break;
				}
				return stringPart + newDigitPart;
			}
			return lastBillNo + "0001";
		}

		public virtual DataTable LoadDataFromSP(string procedureName, string nameSetToTable, string[] paramName, object[] paramValue, int timeout = 30)
		{
			DataTable table = new DataTable();
			SqlConnection mySqlConnection = new SqlConnection(DBUtils.GetDBConnectionString());
			try
			{
				mySqlConnection.Open();
				SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
				mySqlCommand.CommandTimeout = 0;
				mySqlCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
				DataSet myDataSet = new DataSet();
				if (paramName != null)
				{
					for (int i = 0; i < paramName.Length; i++)
					{
						SqlParameter sqlParam = new SqlParameter(paramName[i], paramValue[i]);
						mySqlCommand.Parameters.Add(sqlParam);
					}
				}
				mySqlCommand.ExecuteNonQuery();
				mySqlDataAdapter.Fill(myDataSet, nameSetToTable);
				table = myDataSet.Tables[nameSetToTable];
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				mySqlConnection.Close();
				mySqlConnection.Dispose();
			}
			return table;
		}

		public virtual DataTable LoadDataFromSPNotTimeOut(string procedureName, string nameSetToTable, string[] paramName, object[] paramValue, int timeout = 30)
		{
			DataTable table = new DataTable();
			SqlConnection mySqlConnection = new SqlConnection(DBUtils.GetDBConnectionString());
			try
			{
				mySqlConnection.Open();
				SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
				mySqlCommand.CommandTimeout = 0;
				mySqlCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
				DataSet myDataSet = new DataSet();
				if (paramName != null)
				{
					for (int i = 0; i < paramName.Length; i++)
					{
						SqlParameter sqlParam = new SqlParameter(paramName[i], paramValue[i]);
						mySqlCommand.Parameters.Add(sqlParam);
					}
				}
				mySqlCommand.ExecuteNonQuery();
				mySqlDataAdapter.Fill(myDataSet, nameSetToTable);
				table = myDataSet.Tables[nameSetToTable];
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				mySqlConnection.Close();
				mySqlConnection.Dispose();
			}
			return table;
		}

		public string GenerateNo3(string code)
		{
			string sql = "SELECT TOP 1 MAX(Convert(int," + code + ")) FROM " + tableName + " ";
			string lastBillNo = "";
			SqlConnection conn = new SqlConnection(strcon);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			SqlDataReader reader = null;
			ArrayList result = new ArrayList();
			try
			{
				conn.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				if (reader.Read())
				{
					lastBillNo = reader[0].ToString();
				}
			}
			catch (IndexOutOfRangeException iex)
			{
				throw new Exception("Column \"" + iex.Message + "\" does not exist in table \"" + tableName + "\"");
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
				conn.Close();
				conn.Dispose();
			}
			if (lastBillNo.Length == 0)
			{
				return "00001";
			}
			string digitPart = "";
			string stringPart = lastBillNo;
			int i = lastBillNo.Length - 1;
			while (i >= 0)
			{
				try
				{
					Convert.ToInt32(lastBillNo.Substring(i, 1));
					digitPart = lastBillNo.Substring(i, 1) + digitPart;
					i--;
				}
				catch
				{
					break;
				}
			}
			if (digitPart.Length > 0)
			{
				stringPart = lastBillNo.Substring(0, i + 1);
				string newDigitPart = Convert.ToString(Convert.ToInt32(digitPart) + 1);
				switch (newDigitPart.Length)
				{
				case 1:
					newDigitPart = "0000" + newDigitPart;
					break;
				case 2:
					newDigitPart = "000" + newDigitPart;
					break;
				case 3:
					newDigitPart = "00" + newDigitPart;
					break;
				case 4:
					newDigitPart = "0" + newDigitPart;
					break;
				}
				return stringPart + newDigitPart;
			}
			return lastBillNo + "00001";
		}

		public DateTime GetSystemDate()
		{
			SqlConnection conn = null;
			try
			{
				conn = new SqlConnection(strcon);
				conn.Open();
				SqlCommand cmd = new SqlCommand("spGetDateSystem", conn);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter adapter = new SqlDataAdapter(cmd);
				using (DataSet myDataSet = new DataSet())
				{
					cmd.ExecuteNonQuery();
					adapter.Fill(myDataSet, "SystemDate");
					return Convert.ToDateTime(myDataSet.Tables["SystemDate"].Rows[0][0]);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}
	}
}
