using System.Text;
using Dapper.Contrib.Extensions;
using Library.Extensions;
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

            var (fields, values, parameters) = SqlStorage<T>.GenerateParams(new[] { item });

            return SqlQueryRunner.ExecuteScalar<int>(
                    $"INSERT INTO {tableName} ({string.Join(", ", fields)}) VALUES ({string.Join(", ", values)})",
                    connStr,
                    parameters!
                );
        }

        public int AddRange(IEnumerable<T> items)
        {
            if  (!items.AnySafe())
            {
                return 0;
            }

            var (fields, values, parameters) = SqlStorage<T>.GenerateParams(items);

            return SqlQueryRunner.ExecuteScalar<int>(
                    $"INSERT INTO {tableName} ({string.Join(", ", fields)}) VALUES ({string.Join(", ", values)})",
                    connStr,
                    parameters!
                );
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

        private static (string[], string[], Dictionary<string, object?>) GenerateParams(IEnumerable<T> items)
        {
            var index = 0;
            var itemsLen = items.Count();
            var props = typeof(T).GetProperties();

            var fields = props.Select(p => p.Name).ToArray();
            var values = new string[itemsLen];
            Dictionary<string, object?> parameters = new(itemsLen * props.Length);

            foreach (var item in items)
            {
                var sb = new StringBuilder("(");
                
                foreach (var prop in props)
                {
                    var field = $"{prop.Name}{index}";
                    parameters[field] = prop.GetValue(item);
                    sb.Append($"{field}");
                }

                sb.AppendLine(")");
                values[index++] = sb.ToString();
            }

            return (fields, values, parameters);
        }
    }
}

