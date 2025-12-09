using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Ticket.Utils
{
	public sealed class SqlHelperParameterCache
	{
		private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

		private SqlHelperParameterCache()
		{
		}

		private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (spName == null || spName.Length == 0)
			{
				throw new ArgumentNullException("spName");
			}
			SqlCommand cmd = new SqlCommand(spName, connection);
			cmd.CommandType = CommandType.StoredProcedure;
			connection.Open();
			SqlCommandBuilder.DeriveParameters(cmd);
			connection.Close();
			if (!includeReturnValueParameter)
			{
				cmd.Parameters.RemoveAt(0);
			}
			SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
			cmd.Parameters.CopyTo(discoveredParameters, 0);
			SqlParameter[] array = discoveredParameters;
			foreach (SqlParameter discoveredParameter in array)
			{
				discoveredParameter.Value = DBNull.Value;
			}
			return discoveredParameters;
		}

		private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
		{
			SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
			int i = 0;
			for (int j = originalParameters.Length; i < j; i++)
			{
				clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
			}
			return clonedParameters;
		}

		public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
		{
			if (connectionString == null || connectionString.Length == 0)
			{
				throw new ArgumentNullException("connectionString");
			}
			if (commandText == null || commandText.Length == 0)
			{
				throw new ArgumentNullException("commandText");
			}
			string hashKey = connectionString + ":" + commandText;
			paramCache[hashKey] = commandParameters;
		}

		public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
		{
			if (connectionString == null || connectionString.Length == 0)
			{
				throw new ArgumentNullException("connectionString");
			}
			if (commandText == null || commandText.Length == 0)
			{
				throw new ArgumentNullException("commandText");
			}
			string hashKey = connectionString + ":" + commandText;
			if (!(paramCache[hashKey] is SqlParameter[] cachedParameters))
			{
				return null;
			}
			return CloneParameters(cachedParameters);
		}

		public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
		{
			return GetSpParameterSet(connectionString, spName, includeReturnValueParameter: false);
		}

		public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
		{
			if (connectionString == null || connectionString.Length == 0)
			{
				throw new ArgumentNullException("connectionString");
			}
			if (spName == null || spName.Length == 0)
			{
				throw new ArgumentNullException("spName");
			}
			SqlConnection connection = new SqlConnection(connectionString);
			return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
		}

		internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
		{
			return GetSpParameterSet(connection, spName, includeReturnValueParameter: false);
		}

		internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone();
			return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
		}

		private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (spName == null || spName.Length == 0)
			{
				throw new ArgumentNullException("spName");
			}
			string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
			SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
			if (cachedParameters == null)
			{
				SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
				paramCache[hashKey] = spParameters;
				cachedParameters = spParameters;
			}
			return CloneParameters(cachedParameters);
		}
	}
}
