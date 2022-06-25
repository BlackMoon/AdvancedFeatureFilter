using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.Extensions;

namespace Library
{
    public sealed class DataReaderOptions
    {
        public char Separator { get; set; }

        public bool SkipInitialLine { get; set; } = true;
    }

    public static class DataReader
    {
        public static IAsyncEnumerable<T> ReadFromListAsync<T>(
            IEnumerable<string> list,
            DataReaderOptions? options = null,
            CancellationToken cancellationToken = default) where T : new()
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            using var reader = new StringReader(string.Join("\r\n", list));
            return ReadImplAsync<T>(reader, options, cancellationToken);
        }

        public static IAsyncEnumerable<T> ReadFromStreamAsync<T>(
            Stream stream,
            DataReaderOptions? options = null,
            CancellationToken cancellationToken = default) where T : new()
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            using var reader = new StreamReader(stream);
            return ReadImplAsync<T>(reader, options, cancellationToken);
        }

        public static IAsyncEnumerable<T> ReadFromTextAsync<T>(
            string csv,
            DataReaderOptions? options = null,
            CancellationToken cancellationToken = default) where T : new()
        {
            if (csv == null)
            {
                throw new ArgumentNullException(nameof(csv));
            }

            using var reader = new StringReader(csv);
            return ReadImplAsync<T>(reader, options, cancellationToken);
        }

        private static char AutoDetectSeparator(ReadOnlySpan<char> sampleLine)
        {
            foreach (var ch in sampleLine)
            {
                if (ch == ';' || ch == '\t')
                    return ch;
            }

            return ',';
        }

        public static object ChangeType(object value, Type conversionType)
        {  
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {   
                var nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            } 
           
            return Convert.ChangeType(value, conversionType);
        }

        private static IDictionary<string, int> CreateDefaultHeaders(ReadOnlySpan<char> line, char separator)
        {
            var headers = new Dictionary<string, int>();

            var pos = 0;

            foreach (var _ in line.Split(separator, StringSplitOptions.TrimEntries))
            {
                headers[$"Column{pos + 1}"] = pos++;
            }
           
            return headers;
        }


        private static IDictionary<string, int> GetHeaders(ReadOnlySpan<char> line, char separator)
        {
            var headers = new Dictionary<string, int>();

            try
            {
                var pos = 0;

                foreach (var str in line.Split(separator, StringSplitOptions.TrimEntries)) {
                    headers[str.ToString().ToLowerInvariant()] = pos++;
                }

            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException("Duplicate headers detected.");
            }

            return headers;
        }

        private static void InitializeOptions(ReadOnlySpan<char> line, DataReaderOptions options)
        {
            if (options.Separator == '\0')
            {
                options.Separator = AutoDetectSeparator(line);
            }
        }

        private static async IAsyncEnumerable<T> ReadImplAsync<T>(
            TextReader reader,
            DataReaderOptions? options,
            [EnumeratorCancellation] CancellationToken cancellationToken) where T : new()
        {
            options ??= new DataReaderOptions();

            string? line;
            var index = -1;
            var props = typeof(T).GetProperties();
            IDictionary<string, int>? headerLookup = null;

            while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                index++;
                if (headerLookup == null)
                {
                    InitializeOptions(line.AsSpan(), options);
                    headerLookup = options.SkipInitialLine ? GetHeaders(line.AsSpan(), options.Separator) : CreateDefaultHeaders(line.AsSpan(), options.Separator);

                    if (options.SkipInitialLine)
                    {
                        continue;
                    }
                }

                T obj = new();

                var data = line.Split(options.Separator);
                for (var j = 0; j < props.Length; j++)
                {
                    var prop = props[j];
                    var dataIx = headerLookup.TryGetValue(prop.Name.ToLowerInvariant(), out var colIx) ? colIx : j; 
                    var propType = prop.PropertyType;
                    var val = data[dataIx];

                    try
                    {
                        prop.SetValue(obj, ChangeType(val, propType));
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidCastException($"Failed to convert ({index}, {j}) value", ex);
                    }
                }

                yield return obj;
            }
        }
    }
}

