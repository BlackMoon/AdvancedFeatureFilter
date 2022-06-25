using System;
using System.Data.SqlClient;
using Dapper;

namespace Library
{
    public static class SqlQueryRunner
    {
        public static IEnumerable<T> Execute<T>(string sql, string connStr, IDictionary<string, object> parameters)
        {
            using var conn = new SqlConnection(connStr);

            var dbParams = new DynamicParameters();
            dbParams.AddDynamicParams(parameters);

            var cmd = new CommandDefinition(
                commandText: sql,
                parameters: dbParams);

            return conn.Query<T>(cmd);
        }

        public static async Task<IEnumerable<T>> ExecuteAsync<T>(string sql, string connStr, IDictionary<string, object> parameters, CancellationToken stoppingToken)
        {
            await using var conn = new SqlConnection(connStr);

            var dbParams = new DynamicParameters();
            dbParams.AddDynamicParams(parameters);

            var cmd = new CommandDefinition(
                commandText: sql,
                parameters: dbParams,
                cancellationToken: stoppingToken);

            return await conn.QueryAsync<T>(cmd);
        }

        public static T ExecuteScalar<T>(string sql, string connStr, IDictionary<string, object> parameters)
        {
            using var conn = new SqlConnection(connStr);

            var dbParams = new DynamicParameters();
            dbParams.AddDynamicParams(parameters);

            var cmd = new CommandDefinition(
                commandText: sql,
                parameters: dbParams);

            return conn.ExecuteScalar<T>(cmd);
        }

        public static async Task<T> ExecuteScalarAsync<T>(string sql, string connStr, IDictionary<string, object> parameters, CancellationToken stoppingToken)
        {
            await using var conn = new SqlConnection(connStr);

            var dbParams = new DynamicParameters();
            dbParams.AddDynamicParams(parameters);

            var cmd = new CommandDefinition(
                commandText: sql,
                parameters: dbParams,
                cancellationToken: stoppingToken);

            return await conn.ExecuteScalarAsync<T>(cmd);
        }
    }
}

