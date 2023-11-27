using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
	public static class MySqlConnectionExtensions
	{
		public static async Task<int> GetMaxAllowedPacket(this MySqlConnection mySqlConnection)
		{
			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText = $@"
					SHOW VARIABLES LIKE 'MAX_ALLOWED_PACKET';
				";

				var reader = await mySqlCommand.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
				var result = -1;

				if (await reader.ReadAsync().ConfigureAwait(false))
				{
					var value = reader[1];

					if (value != null)
					{
						result = Convert.ToInt32(value);
					}
				}

				await reader.CloseAsync().ConfigureAwait(false);

				return result;
			}
		}
		
		public static async Task<List<Tuple<Type, string>>> GetTableColumns(this MySqlConnection mySqlConnection, string table)
		{
			var columns = new List<Tuple<Type, string>>();

			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText = $@"
					SELECT *
					FROM {table}
					LIMIT 1
				";

				await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				var mySqlDataReader = await mySqlCommand.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
				var fieldCount = mySqlDataReader.FieldCount;

				if (await mySqlDataReader.ReadAsync())
				{
					for (var i = 0; i < fieldCount; i++)
					{
						var fieldType = mySqlDataReader.GetFieldType(i);
						var name = mySqlDataReader.GetName(i);
						var item = new Tuple<Type, string>(fieldType, name);

						columns.Add(item);
					}
				}

                await mySqlDataReader.CloseAsync().ConfigureAwait(false);
			}

			return columns;
		}

		public static async Task<List<string>> GetTables(this MySqlConnection mySqlConnection)
		{
			var database = mySqlConnection.Database;
			var tables = new List<string>();

			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText = $@"
					SELECT TABLE_NAME
					FROM INFORMATION_SCHEMA.TABLES
					WHERE TABLE_SCHEMA = '{database}'
				";
				//mySqlCommand.Connection = mySqlConnection;

				await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				var mySqlDataReader = await mySqlCommand.ExecuteReaderAsync().ConfigureAwait(false);

				while (await mySqlDataReader.ReadAsync().ConfigureAwait(false))
				{
					var tableName = mySqlDataReader.GetString(mySqlDataReader.GetOrdinal("TABLE_NAME"));

					tables.Add($"{tableName}");
				}

                await mySqlDataReader.CloseAsync().ConfigureAwait(false);
			}

			return tables;
		}

		public static async Task<List<Dictionary<string, object>>> GetRows(this MySqlConnection mySqlConnection, string table)
		{
			var rows = new List<Dictionary<string, object>>();

			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText = $@"
					SELECT *
					FROM {table}
				";

				await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				var mySqlDataReader = await mySqlCommand.ExecuteReaderAsync().ConfigureAwait(false);
				var fieldCount = mySqlDataReader.FieldCount;

				while (await mySqlDataReader.ReadAsync().ConfigureAwait(false))
				{
					var item = new Dictionary<string, object>(fieldCount);

					for (var i = 0; i < fieldCount; i++)
					{
						var key = mySqlDataReader.GetName(i);
						object value = null;

						if (i >= 0)
						{
							var isDBNull = await mySqlDataReader.IsDBNullAsync(i).ConfigureAwait(false);

							if (!isDBNull)
							{
								value = mySqlDataReader.GetValue(i);
							}
						}

						item.Add(key, value);
					}


					rows.Add(item);
				}

                await mySqlDataReader.CloseAsync().ConfigureAwait(false);
			}

			return rows;
		}

		public static async Task<bool> HasLocalInfile(this MySqlConnection mySqlConnection)
		{
			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText = $@"
					SHOW VARIABLES LIKE 'LOCAL_INFILE';
				";

				var reader = await mySqlCommand.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
				var result = false;

				if (await reader.ReadAsync().ConfigureAwait(false))
				{
					var value = reader[1];

					if (value != null)
					{
						var parsedValue = value.ToString();
						parsedValue = parsedValue.ToUpper();
						result = parsedValue == "ON";
					}
				}

				await reader.CloseAsync().ConfigureAwait(false);

				return result;
			}
		}

		public static async Task<bool> HasStrictTransTables(this MySqlConnection mySqlConnection)
		{
			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText = $@"
					SHOW VARIABLES LIKE 'SQL_MODE';
				";

				var reader = await mySqlCommand.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
				var result = false;

				if (await reader.ReadAsync().ConfigureAwait(false))
				{
					var value = reader[1];

					if (value != null)
					{
						var parsedValue = value.ToString();
						parsedValue = parsedValue.ToUpper();
						result = parsedValue.Contains("STRICT_TRANS_TABLES");
					}
				}

				await reader.CloseAsync().ConfigureAwait(false);

				return result;
			}
		}

		public static async Task<bool> IsValidTable(this MySqlConnection mySqlConnection, string table)
		{
			var database = mySqlConnection.Database;
			var isValidTable = false;

			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText += $@"
					SELECT COUNT(1)
					FROM INFORMATION_SCHEMA.TABLES
					WHERE TABLE_NAME = @TableName AND TABLE_SCHEMA = @TableSchema
				";
				//mySqlCommand.Connection = mySqlConnection;

                await mySqlCommand.PrepareAsync().ConfigureAwait(false);
				mySqlCommand.Parameters.Add("@TableName", MySqlDbType.VarChar);
				mySqlCommand.Parameters.Add("@TableSchema", MySqlDbType.VarChar);

				mySqlCommand.Parameters["@TableName"].Value = table;
				mySqlCommand.Parameters["@TableSchema"].Value = database;

				var count = await mySqlCommand.ExecuteScalarAsync().ConfigureAwait(false);
				var count2 = Convert.ToInt32(count);

				if (count2 > 0)
				{
					isValidTable = true;
				}
			}

			return isValidTable;
		}

		public static async Task<int> SetForeignKeyChecks(this MySqlConnection mySqlConnection, bool isEnabled)
		{
			var id = -1;

			using (var mySqlTransaction = await mySqlConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false))
			{
				using var mySqlCommand = mySqlConnection.CreateCommand();

				if (isEnabled)
				{
					mySqlCommand.CommandText = $@"
						SET FOREIGN_KEY_CHECKS = 1;
					";
				}
				else
				{
					mySqlCommand.CommandText = $@"
						SET FOREIGN_KEY_CHECKS = 0;
					";
				}
				mySqlCommand.Transaction = mySqlTransaction;

				await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				id = await mySqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

				await mySqlTransaction.CommitAsync().ConfigureAwait(false);
			}

			return id;
		}

		public static async Task<int> SetLocalInfile(this MySqlConnection mySqlConnection, bool isEnabled)
		{
			var id = -1;

			using (var mySqlTransaction = await mySqlConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false))
			{
				using var mySqlCommand = mySqlConnection.CreateCommand();

				if (isEnabled)
				{
					mySqlCommand.CommandText = $@"
						SET GLOBAL LOCAL_INFILE = 1;
					";
				}
				else
				{
					mySqlCommand.CommandText = $@"
						SET GLOBAL LOCAL_INFILE = 0;
					";
				}
				mySqlCommand.Transaction = mySqlTransaction;

				await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				id = await mySqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

				await mySqlTransaction.CommitAsync().ConfigureAwait(false);
			}

			return id;
		}

		public static async Task<int> SetMaxAllowedPacket(this MySqlConnection mySqlConnection, int value)
		{
			var id = -1;

			using (var mySqlTransaction = mySqlConnection.BeginTransaction(IsolationLevel.ReadCommitted))
			{
				using var mySqlCommand = mySqlConnection.CreateCommand();
				mySqlCommand.CommandText = $@"
					SET GLOBAL MAX_ALLOWED_PACKET = {value};
				";
				mySqlCommand.Transaction = mySqlTransaction;

				await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				id = await mySqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

				await mySqlTransaction.CommitAsync().ConfigureAwait(false);
			}

			return id;
		}
		
		public static async Task<int> SetStrictTransTables(this MySqlConnection mySqlConnection, bool isEnabled)
		{
			var id = -1;
			var sqlMode = "";

			using (var mySqlCommand = mySqlConnection.CreateCommand())
			{
				mySqlCommand.CommandText = $@"
					SHOW VARIABLES LIKE 'SQL_MODE';
				";

				var reader = await mySqlCommand.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);

				if (await reader.ReadAsync().ConfigureAwait(false))
				{
					var value = reader[1];

					if (value != null)
					{
						var parsedValue = value.ToString();
						sqlMode = parsedValue.ToUpper();
					}
				}

				await reader.CloseAsync().ConfigureAwait(false);
			}

			using (var mySqlTransaction = mySqlConnection.BeginTransaction(IsolationLevel.ReadCommitted))
			{
				using var mySqlCommand = mySqlConnection.CreateCommand();

				if (isEnabled)
				{
					if (!sqlMode.Contains("STRICT_TRANS_TABLES"))
					{
						var sqlModeSplit = sqlMode.Split(",");
                        var sqlModeList = new List<string>(sqlModeSplit)
                        {
                            "STRICT_TRANS_TABLES"
                        };

                        mySqlCommand.CommandText = $@"
							SET GLOBAL SQL_MODE = '{string.Join(",", sqlModeList)}';
						";
					}
				}
				else
				{
					if (sqlMode.Contains("STRICT_TRANS_TABLES"))
					{
						var sqlModeSplit = sqlMode.Split(",");
						var sqlModeList = new List<string>(sqlModeSplit);

						sqlModeList.Remove("STRICT_TRANS_TABLES");

						mySqlCommand.CommandText = $@"
							SET GLOBAL SQL_MODE = '{string.Join(",", sqlModeList)}';
						";
					}
				}

				//mySqlCommand.Connection = mySqlConnection;
				mySqlCommand.Transaction = mySqlTransaction;

				await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				id = await mySqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

				await mySqlTransaction.CommitAsync().ConfigureAwait(false);
			}

			return id;
		}

		public static async Task<int> SetUniqueChecks(this MySqlConnection mySqlConnection, bool isEnabled)
		{
			var id = -1;

			using (var mySqlTransaction = mySqlConnection.BeginTransaction(IsolationLevel.ReadCommitted))
			{
				using var mySqlCommand = mySqlConnection.CreateCommand();

				if (isEnabled)
				{
					mySqlCommand.CommandText = $@"
						SET UNIQUE_CHECKS = 1;
					";
				}
				else
				{
					mySqlCommand.CommandText = $@"
						SET UNIQUE_CHECKS = 0;
					";
				}

				//mySqlCommand.Connection = mySqlConnection;
				mySqlCommand.Transaction = mySqlTransaction;

                await mySqlCommand.PrepareAsync().ConfigureAwait(false);

				id = await mySqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                await mySqlTransaction.CommitAsync().ConfigureAwait(false);
			}

			return id;
		}

		public static async Task BulkInsert(this MySqlConnection mySqlConnection, DataTable dataTable, string table, int commandTimeout = 0)
		{
			using var mySqlTransaction = await mySqlConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);

			BulkInsert(mySqlConnection, dataTable, table, commandTimeout, mySqlTransaction);
		}

		public static void BulkInsert(this MySqlConnection mySqlConnection, DataTable dataTable, string table, int commandTimeout = 0, MySqlTransaction mySqlTransaction = null)
		{
			using var mySqlCommand = mySqlConnection.CreateCommand();
			mySqlCommand.CommandText = $@"
				SELECT *
				FROM {table}
			";
			mySqlCommand.CommandTimeout = commandTimeout;
			//mySqlCommand.Connection = mySqlConnection;
			mySqlCommand.Transaction = mySqlTransaction;

			using var mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand)
			{
				UpdateBatchSize = 1000
			};

			using var mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter)
			{
				SetAllValues = true
			};

			try
			{
				mySqlDataAdapter.Update(dataTable);
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				mySqlCommandBuilder.Dispose();
				mySqlDataAdapter.Dispose();
			}
		}
	}
}
