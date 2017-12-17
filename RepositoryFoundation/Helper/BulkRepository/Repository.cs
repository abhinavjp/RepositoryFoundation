using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace RepositoryFoundation.Helper.BulkRepository
{
    public class Repository
    {
        private readonly DataTable _dataTable;
        private readonly string _connectionString;
        private readonly string _tempTableName;
        private readonly string _destinationTableName;
        private readonly string _primaryKeyColumnName;
        public Repository(DataTable dataTable, string destinationTableName, string connectionString, string primaryKeyColumnName)
        {
            _dataTable = dataTable;
            _connectionString = connectionString;
            _tempTableName = $"#UpsertTable_{DateTime.UtcNow.ToFileTimeUtc()}";
            _destinationTableName = destinationTableName;
            _primaryKeyColumnName = primaryKeyColumnName;
        }

        private void ValidateRequiredFields()
        {
            if (_dataTable == null || _dataTable.Rows.Count == 0)
                throw new ArgumentException("No data found to insert", nameof(_dataTable));
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new ArgumentException("Please provide a connection string"
                    , nameof(_connectionString));
            if (string.IsNullOrWhiteSpace(_destinationTableName))
                throw new ArgumentException("Please provide a Table name where you want to insert data"
                    , nameof(_destinationTableName));
            if (string.IsNullOrWhiteSpace(_primaryKeyColumnName))
                throw new ArgumentException("Please provide a primary key column name of the table into which data needs to be inserted"
                    , nameof(_primaryKeyColumnName));
        }

        public void BulkUpsert()
        {
            ValidateRequiredFields();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                InsertDataToTempTable(connection);
                var query = CreateMergeQuery();

                using (IDbCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateTempTable(SqlConnection connection)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT * INTO {_tempTableName} FROM @tempTable");


            using (var tempTable = new DataTable())
            {
                foreach (DataColumn dataColumn in _dataTable.Columns)
                {
                    tempTable.Columns.Add(JsonConvert.DeserializeObject<DataColumn>(JsonConvert.SerializeObject(dataColumn)));
                }
                var parameter = new SqlParameter("@tempTable", SqlDbType.Structured) { Value = tempTable };
                using (IDbCommand cmd = new SqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.Add(parameter);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private void InsertDataToTempTable(SqlConnection connection)
        {
            CreateTempTable(connection);
            using (var bulk = new SqlBulkCopy(connection))
            {
                bulk.DestinationTableName = _tempTableName;
                bulk.WriteToServer(_dataTable);
            }
        }

        private string CreateMergeQuery()
        {
            var query = new StringBuilder();
            query.AppendLine($"MERGE INTO {_destinationTableName} AS Target ");
            query.AppendLine($"USING {_tempTableName} AS Source ");
            query.AppendLine("ON");
            query.AppendLine($"Target.{_primaryKeyColumnName}=Source.{_primaryKeyColumnName} ");
            query.AppendLine("WHEN MATCHED THEN ");
            query.AppendLine("UPDATE SET ");

            foreach (DataColumn column in _dataTable.Columns)
            {
                if (column.ColumnName == _primaryKeyColumnName)
                    continue;
                query.AppendLine($"Target.{column.ColumnName}=Source.{column.ColumnName} ");
            }
            query.AppendLine("WHEN NOT MATCHED THEN ");
            query.AppendLine(
                $"INSERT ({string.Join(",", _dataTable.Columns.Cast<DataColumn>().Where(w=>w.ColumnName!=_primaryKeyColumnName).Select(s => s.ColumnName))}) VALUES ");
            query.AppendLine($"({string.Join(", ", _dataTable.Columns.Cast<DataColumn>().Where(w => w.ColumnName != _primaryKeyColumnName).Select(s => $"Source.{s.ColumnName}"))})");

            return query.ToString();
        }
    }
}
