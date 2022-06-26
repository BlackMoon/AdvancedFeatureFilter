using System;
using System.Reflection;
using Dapper.Contrib.Extensions;
using Library.Rules;
using Microsoft.Extensions.Configuration;

namespace Library.Storage
{
    public class SqlStorage<T> : IStorage<T> where T : BaseRule
    {
        private readonly string connStr;
        private readonly string tableName;
        

        public SqlStorage(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("sampleDb");
            tableName = SqlMapperExtensions.TableNameMapper(typeof(T));
        }

        public int Add(T item)
        {
            if (item == null)
            {
                return 0;
            }

            var props = typeof(T).GetProperties();

            var fields = new List<string>(props.Length);
            Dictionary<string, object?> parameters = new Dictionary<string, object?>(props.Length);
            foreach(var prop in props)
            {
                var field = prop.Name;
                fields.Add(field);
                parameters[field] = prop.GetValue(item);
            }

            var columns = string.Join(", ", fields);
            var values = string.Join(", ", fields.Select(f => $"@{f}"));

            return SqlQueryRunner.ExecuteScalar<int>(
                    $"INSERT INTO {tableName} ({columns}) VALUES ({values})",
                    connStr,
                    parameters!
                );
        }

        public int AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public T? FindByHashCode(int[] hashes, IComparer<T>? comparer)
        {
            return SqlQueryRunner.Execute<T>(
                    $"SELECT TOP 1 r.* FROM {tableName} r WHERE r.hash IN @hashes ORDER BY r.priority DESC",
                    connStr,
                    new Dictionary<string, object> { { "hashes", hashes } })
                .FirstOrDefault();
        }

        public async Task<T?> FindByHashCodeAsync(int[] hashes, IComparer<T>? comparer, CancellationToken cancellationToken)
        {
            return (
                await SqlQueryRunner.ExecuteAsync<T>(
                    $"SELECT TOP 1 r.* FROM {tableName} r WHERE r.hash IN @hashes ORDER BY r.priority DESC",
                    connStr,
                    new Dictionary<string, object> { { "hashes", hashes } },
                    cancellationToken)
                )
                .FirstOrDefault();
        }
    }
}

