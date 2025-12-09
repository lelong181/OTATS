using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Ticket.Utils
{
	public class ProcessTransaction
	{
		private SqlConnection cnn;

		private SqlTransaction tran;

		private SqlCommand cmd;

		private SqlDataAdapter da;

		public ProcessTransaction()
		{
			cnn = new SqlConnection(DBUtils.GetDBConnectionString());
		}

		public ProcessTransaction(int type)
		{
			switch (type)
			{
			case 1:
				cnn = new SqlConnection(DBUtils.GetBookingDBConnectionString());
				break;
			case 2:
				cnn = new SqlConnection(DBUtils.GetFastDBConnectionString());
				break;
			default:
				cnn = new SqlConnection(DBUtils.GetDBConnectionString());
				break;
			}
		}

		public static string getClassName(string tableName)
		{
			string result = "";
			return tableName + "Model";
		}

		private static object PopulateObject(DataRow dr, object model)
		{
			PropertyInfo[] propertiesName = model.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				object value = dr[propertiesName[i].Name];
				if (value != DBNull.Value)
				{
					propertiesName[i].SetValue(model, value, null);
				}
			}
			return model;
		}

		private static object PopulateObject(DataRow dr, string fullname)
		{
			object model = Activator.CreateInstance(Type.GetType(fullname));
			return PopulateObject(dr, model);
		}

		public static BaseModel PopulateModel(DataRow dr, string name)
		{
			return (BaseModel)PopulateObject(dr, "EzTicket." + name);
		}

		public DataTable Select(string strComm)
		{
			try
			{
				cmd = new SqlCommand("spGenSearchWithCommand", cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", strComm));
				cmd.ExecuteNonQuery();
				da = new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();
				da.Fill(ds);
				if (ds.Tables != null && ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
				return new DataTable();
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void ExecuteNonQuery(string strComm)
		{
			try
			{
				cmd = new SqlCommand("spGenSearchWithCommand", cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", strComm));
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public DataTable getTable(string procedureName, SqlParameter mySqlParameter, string nameSetToTable)
		{
			DataTable table = new DataTable();
			try
			{
				cmd = new SqlCommand(procedureName, cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				da = new SqlDataAdapter(cmd);
				DataSet myDataSet = new DataSet();
				if (mySqlParameter != null)
				{
					cmd.Parameters.Add(mySqlParameter);
				}
				cmd.ExecuteNonQuery();
				da.Fill(myDataSet, nameSetToTable);
				return myDataSet.Tables[nameSetToTable];
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public DataTable getTable(string procedureName, string nameSetToTable, params SqlParameter[] mySqlParameter)
		{
			DataTable table = new DataTable();
			try
			{
				cmd = new SqlCommand(procedureName, cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				da = new SqlDataAdapter(cmd);
				DataSet myDataSet = new DataSet();
				for (int i = 0; i < mySqlParameter.Length; i++)
				{
					cmd.Parameters.Add(mySqlParameter[i]);
				}
				cmd.ExecuteNonQuery();
				da.Fill(myDataSet, nameSetToTable);
				return myDataSet.Tables[nameSetToTable];
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public DataTable getTable(string procedureName, string nameSetToTable, string[] paramName, object[] paramValue)
		{
			DataTable table = new DataTable();
			try
			{
				cmd = new SqlCommand(procedureName, cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				da = new SqlDataAdapter(cmd);
				DataSet myDataSet = new DataSet();
				for (int i = 0; i < paramName.Length; i++)
				{
					SqlParameter sqlParam = new SqlParameter(paramName[i], paramValue[i]);
					cmd.Parameters.Add(sqlParam);
				}
				cmd.ExecuteNonQuery();
				da.Fill(myDataSet, nameSetToTable);
				return myDataSet.Tables[nameSetToTable];
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void ExecSP(string procedureName, SqlParameter mySqlParameter)
		{
			try
			{
				cmd = new SqlCommand(procedureName, cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(mySqlParameter);
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public int ExecSP(string procedureName, SqlParameter[] mySqlParameter)
		{
			int result = 0;
			try
			{
				cmd = new SqlCommand(procedureName, cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (SqlParameter param in mySqlParameter)
				{
					cmd.Parameters.Add(param);
				}
				cmd.ExecuteNonQuery();
				if (cmd.Parameters["@result"] != null)
				{
					result = Convert.ToInt32(cmd.Parameters["@result"].Value.ToString());
				}
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
			return result;
		}

		public virtual DataTable LoadDataFromSP(string procedureName, string nameSetToTable, string[] paramName, object[] paramValue)
		{
			DataTable table = new DataTable();
			try
			{
				cmd = new SqlCommand(procedureName, cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(cmd);
				DataSet myDataSet = new DataSet();
				if (paramName != null)
				{
					for (int i = 0; i < paramName.Length; i++)
					{
						SqlParameter sqlParam = new SqlParameter(paramName[i], paramValue[i]);
						cmd.Parameters.Add(sqlParam);
					}
				}
				cmd.ExecuteNonQuery();
				mySqlDataAdapter.Fill(myDataSet, nameSetToTable);
				table = myDataSet.Tables[nameSetToTable];                

            }
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
			return table;
		}

		public ArrayList FindByAttribute(string tableName, string fieldName, string fieldValue)
		{
			ArrayList result = new ArrayList();
			try
			{
				string sql = "select * from [" + tableName + "] with(nolock) where [" + fieldName + "]=N'" + fieldValue + "'";
				cmd = new SqlCommand(sql, cnn, tran);
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sql;
				using (SqlDataAdapter da = new SqlDataAdapter(cmd))
				{
					DataSet ds = new DataSet();
					da.Fill(ds, "TABLE");
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						DataRow dr = ds.Tables[0].Rows[i];
						result.Add(PopulateModel(dr, getClassName(tableName)));
					}
				}
				return result;
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public ArrayList FindByExpression(string tableName, Expression exp)
		{
			ArrayList result = new ArrayList();
			try
			{
				string sql = DBUtils.SQLSelect(tableName, exp);
				cmd = new SqlCommand(sql, cnn, tran);
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sql;
				using (SqlDataAdapter da = new SqlDataAdapter(cmd))
				{
					DataSet ds = new DataSet();
					da.Fill(ds, "TABLE");
					string classname = getClassName(tableName);
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						DataRow dr = ds.Tables[0].Rows[i];
						result.Add(PopulateModel(dr, getClassName(tableName)));
					}
				}
				return result;
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public BaseModel FindByPK(string tableName, long ID)
		{
			ArrayList result = new ArrayList();
			try
			{
				string sql = "select * from [" + tableName + "] with(nolock) where ID=" + ID;
				cmd = new SqlCommand(sql, cnn, tran);
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sql;
				using (SqlDataAdapter da = new SqlDataAdapter(cmd))
				{
					DataSet ds = new DataSet();
					da.Fill(ds, "TABLE");
					string classname = getClassName(tableName);
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						DataRow dr = ds.Tables[0].Rows[i];
						result.Add(PopulateModel(dr, getClassName(tableName)));
					}
				}
				if (result.Count > 0)
				{
					return (BaseModel)result[0];
				}
				return null;
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public decimal Insert(BaseModel baseModel)
		{
			string TableName = baseModel.GetType().Name.Substring(0, baseModel.GetType().Name.Length - 5);
			string sql = DBUtils.SQLInsert(baseModel);
			cmd = new SqlCommand(sql, cnn, tran);
			cmd.CommandType = CommandType.Text;
			PropertyInfo[] propertiesName = baseModel.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				SqlDbType dbType = DBUtils.ConvertToSQLType(propertiesName[i].PropertyType);
				object value = propertiesName[i].GetValue(baseModel, null);
				if (!propertiesName[i].Name.Equals("ID"))
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
			try
			{
				return (decimal)cmd.ExecuteScalar();
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void InsertNoneReturn(BaseModel baseModel)
		{
			string TableName = baseModel.GetType().Name.Substring(0, baseModel.GetType().Name.Length - 5);
			string sql = DBUtils.SQLInsert(baseModel);
			cmd = new SqlCommand(sql, cnn, tran);
			cmd.CommandType = CommandType.Text;
			PropertyInfo[] propertiesName = baseModel.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				object value = propertiesName[i].GetValue(baseModel, null);
				if (!propertiesName[i].Name.Equals("ID"))
				{
					if (value != null)
					{
						cmd.Parameters.Add("@" + propertiesName[i].Name, DBUtils.ConvertToSQLType(propertiesName[i].PropertyType)).Value = value;
					}
					else
					{
						cmd.Parameters.Add("@" + propertiesName[i].Name, DBUtils.ConvertToSQLType(propertiesName[i].PropertyType)).Value = "";
					}
				}
			}
			try
			{
				cmd.ExecuteScalar();
			}
			catch (SqlException e)
			{
				tran.Rollback();
				throw new Exception(e.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void Update(BaseModel baseModel)
		{
			string TableName = baseModel.GetType().Name.Substring(0, baseModel.GetType().Name.Length - 5);
			string sql = DBUtils.SQLUpdate(baseModel);
			cmd = new SqlCommand(sql, cnn, tran);
			cmd.CommandType = CommandType.Text;
			PropertyInfo[] propertiesName = baseModel.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				SqlDbType dbType = DBUtils.ConvertToSQLType(propertiesName[i].PropertyType);
				object value = propertiesName[i].GetValue(baseModel, null);
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
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (SqlException se)
			{
				tran.Rollback();
				throw new Exception("Update " + baseModel.GetType().Name + " error :" + se.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void UpdateAttribute(string TableName, string[] FieldExpression, object[] ValueExpression, string[] FieldChange, object[] ValueChange)
		{
			string[] str = new string[5] { "", "", "", "", "" };
			string command = "Update " + TableName + " Set ";
			for (int j = 0; j < FieldChange.Length; j++)
			{
				command = ((j == FieldChange.Length - 1) ? (command + FieldChange[j] + "=@" + FieldChange[j]) : (command + FieldChange[j] + "=@" + FieldChange[j] + ","));
			}
			command += " where ";
			if (FieldExpression.Length == 1)
			{
				command = command + FieldExpression[0] + "=@" + FieldExpression[0];
			}
			else
			{
				for (int l = 0; l < FieldExpression.Length; l++)
				{
					command = ((l == FieldExpression.Length - 1) ? (command + FieldExpression[l] + "=@" + FieldExpression[l]) : (command + FieldExpression[l] + "=@" + FieldExpression[l] + " And "));
				}
			}
			try
			{
				cmd = new SqlCommand(command, cnn, tran);
				cmd.CommandType = CommandType.Text;
				for (int i = 0; i < FieldChange.Length; i++)
				{
					SqlParameter param = new SqlParameter(FieldChange[i], ValueChange[i]);
					cmd.Parameters.Add(param);
				}
				for (int k = 0; k < FieldExpression.Length; k++)
				{
					SqlParameter param = new SqlParameter(FieldExpression[k], ValueExpression[k]);
					cmd.Parameters.Add(param);
				}
				cmd.ExecuteNonQuery();
			}
			catch (SqlException se)
			{
				tran.Rollback();
				throw new Exception("Update error :" + se.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void UpdateCommand(string command)
		{
			try
			{
				cmd = new SqlCommand("spSearchAllForTrans", cnn, tran);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", command));
				cmd.ExecuteNonQuery();
			}
			catch (SqlException se)
			{
				tran.Rollback();
				throw new Exception("Update error :" + se.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void Delete(string tableName, int PKID)
		{
			string sql = "delete from " + tableName + " where ID=" + PKID;
			cmd = new SqlCommand(sql, cnn, tran);
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (SqlException se)
			{
				tran.Rollback();
				throw new Exception("Delete " + tableName + " error :" + se.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public void DeleteByAttribute(string tableName, string FieldName, string FieldValue)
		{
			string sql = "delete from " + tableName + " where " + FieldName + "=" + FieldValue;
			cmd = new SqlCommand(sql, cnn, tran);
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (SqlException se)
			{
				tran.Rollback();
				throw new Exception("Delete " + tableName + " error :" + se.Message);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				throw new Exception(ex.Message);
			}
		}

		public static void ExcuteNonQuery(string SPName, string ParamName, object ParamValue)
		{
		}

		public DateTime GetSystemDate()
		{
			try
			{
				return Convert.ToDateTime(Select("SELECT GETDATE() AS SystemDate").Rows[0][0]);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public void OpenConnection()
		{
			cnn.Open();
		}

		public void BeginTransaction()
		{
			tran = cnn.BeginTransaction();
		}

		public void CommitTransaction()
		{
			tran.Commit();
		}

		public void RollBack()
		{
			tran.Rollback();
			CloseConnection();
		}

		public void CloseConnection()
		{
			if (cnn.State == ConnectionState.Open)
			{
				cnn.Close();
			}
		}
	}
}
