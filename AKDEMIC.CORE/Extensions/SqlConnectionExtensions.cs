using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class SqlConnectionExtensions
	{
		public static async Task<List<string>> GetSystemColumns(this SqlConnection sqlConnection, string table)
		{
			var columns = new List<string>();
			var tableSplit = table.Split(".");
            var tableName = tableSplit.Length >= 2 ? tableSplit[1] : tableSplit[0];
            var tableSchema = tableSplit.Length >= 2 ? tableSplit[0] : "dbo";

            using (var sqlCommand = sqlConnection.CreateCommand())
			{
				sqlCommand.CommandText = $@"
					SELECT
						sys.columns.name AS column_name
					FROM sys.columns
					INNER JOIN sys.tables
					ON sys.columns.object_id = sys.tables.object_id
					INNER JOIN sys.schemas
					ON sys.tables.schema_id = sys.schemas.schema_id
					WHERE sys.schemas.name = '{tableSchema}' AND sys.tables.name = '{tableName}'
					ORDER BY column_id
				";
				sqlCommand.Connection = sqlConnection;

				sqlCommand.Prepare();

				var sqlDataReader = await sqlCommand.ExecuteReaderAsync();

				while (await sqlDataReader.ReadAsync())
				{
					var column = sqlDataReader.GetString(sqlDataReader.GetOrdinal("column_name"));

					columns.Add(column);
				}

				sqlDataReader.Close();
			}

			return columns;
		}

		public static async Task<List<Tuple<Type, string>>> GetTableColumns(this SqlConnection sqlConnection, string table)
		{
			var columns = new List<Tuple<Type, string>>();

			using (var sqlCommand = sqlConnection.CreateCommand())
			{
				sqlCommand.CommandText = $@"
					SELECT TOP 1 *
					FROM {table}
				";
				sqlCommand.Connection = sqlConnection;

				sqlCommand.Prepare();

				var sqlDataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.SingleRow);
				var fieldCount = sqlDataReader.FieldCount;

				if (await sqlDataReader.ReadAsync())
				{
					for (var i = 0; i < fieldCount; i++)
					{
						var fieldType = sqlDataReader.GetFieldType(i);
						var name = sqlDataReader.GetName(i);
						var item = new Tuple<Type, string>(fieldType, name);

						columns.Add(item);
					}
				}

				sqlDataReader.Close();
			}

			return columns;
		}

		public static async Task<List<string>> GetTables(this SqlConnection sqlConnection)
		{
			var database = sqlConnection.Database;
			var tables = new List<string>();

			using (var sqlCommand = sqlConnection.CreateCommand())
			{
				sqlCommand.CommandText = $@"
							SELECT
								TABLE_NAME,
								TABLE_SCHEMA
							FROM [{database}].INFORMATION_SCHEMA.TABLES
						";
				sqlCommand.Connection = sqlConnection;

				sqlCommand.Prepare();

				var sqlDataReader = await sqlCommand.ExecuteReaderAsync();

				while (await sqlDataReader.ReadAsync())
				{
					var tableName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("TABLE_NAME"));
					var tableSchema = sqlDataReader.GetString(sqlDataReader.GetOrdinal("TABLE_SCHEMA"));

					tables.Add($"{tableSchema}.{tableName}");
				}

				sqlDataReader.Close();
			}

			return tables;
		}

		public static async Task<List<Dictionary<string, object>>> GetRows(this SqlConnection sqlConnection, string table)
		{
			var rows = new List<Dictionary<string, object>>();

			using (var sqlCommand = sqlConnection.CreateCommand())
			{
				sqlCommand.CommandText = $@"
					SELECT *
					FROM {table}
				";
				sqlCommand.Connection = sqlConnection;

				sqlCommand.Prepare();

				var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
				var fieldCount = sqlDataReader.FieldCount;

				while (await sqlDataReader.ReadAsync())
				{
					var item = new Dictionary<string, object>(fieldCount);

					for (var i = 0; i < fieldCount; i++)
					{
						var key = sqlDataReader.GetName(i);
						object value = null;

						if (i >= 0)
						{
							var isDBNull = await sqlDataReader.IsDBNullAsync(i);

							if (!isDBNull)
							{
								value = sqlDataReader.GetValue(i);
							}
						}

						item.Add(key, value);
					}


					rows.Add(item);
				}

				sqlDataReader.Close();
			}

			return rows;
        }

        public static async Task<bool> IsValidTable(this SqlConnection sqlConnection, string table)
        {
            var database = sqlConnection.Database;
            var tableSplit = table.Split(".");
            var tableName = tableSplit.Length >= 2 ? tableSplit[1] : tableSplit[0];
            var tableSchema = tableSplit.Length >= 2 ? tableSplit[0] : "dbo";
            var isValidTable = false;

            using (var sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText += $@"
					SELECT COUNT(1)
					FROM [{database}].INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = @TableName AND TABLE_SCHEMA = @TableSchema
				";
                sqlCommand.Connection = sqlConnection;

                sqlCommand.Prepare();
                sqlCommand.Parameters.Add("@TableName", SqlDbType.VarChar);
                sqlCommand.Parameters.Add("@TableSchema", SqlDbType.VarChar);

                sqlCommand.Parameters["@TableName"].Value = tableName;
                sqlCommand.Parameters["@TableSchema"].Value = tableSchema;
                
                var count = await sqlCommand.ExecuteScalarAsync();
                var count2 = Convert.ToInt32(count);

                if (count2 > 0)
                {
                    isValidTable = true;
                }
            }

            return isValidTable;
        }

        public static async Task<int> SetAllConstraints(this SqlConnection sqlConnection, bool isEnabled)
        {
            var rows = -1;

            using (var sqlTransaction = sqlConnection.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    if (isEnabled)
                    {
                        sqlCommand.CommandText = $@"
                            EXEC sp_msforeachtable 'ALTER TABLE ? CHECK CONSTRAINT all'
                        ";
                    }
                    else
                    {
                        sqlCommand.CommandText = $@"
                            EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'
                        ";
                    }

                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Transaction = sqlTransaction;

                    sqlCommand.Prepare();
                    
                    rows = await sqlCommand.ExecuteNonQueryAsync();

                    sqlTransaction.Commit();
                }
            }

            return rows;
        }

        public static async Task<int> SetTableConstraints(this SqlConnection sqlConnection, string table, bool isEnabled)
        {
            var id = -1;

            using (var sqlTransaction = sqlConnection.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    if (isEnabled)
                    {
                        sqlCommand.CommandText = $@"
                            ALTER TABLE {table} CHECK CONSTRAINT ALL
                        ";
                    }
                    else
                    {
                        sqlCommand.CommandText = $@"
                            ALTER TABLE {table} NOCHECK CONSTRAINT ALL
                        ";
                    }

                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Transaction = sqlTransaction;

                    sqlCommand.Prepare();

                    id = await sqlCommand.ExecuteNonQueryAsync();

                    sqlTransaction.Commit();
                }
            }

            return id;
        }
    }
}
