using System.Text;
using System.Text.Json;
using Library.Extensions;
using Microsoft.Extensions.Caching.Distributed;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Storage
{
    public class CacheStorage<T> : IStorage<T> where T : IComparable<T>
    {
        private readonly IDistributedCache cache;

        public CacheStorage(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public int Add(T item)
        {
            if (item == null)
            {
                return 0;
            }

            var key = item.GetHashCode().ToString();
            var objectFromCache = cache.Get(key);

            var sortedList = DeserializeObject(objectFromCache) ?? new List<T>();
            sortedList.AddSorted(item);

            var objectToCache = JsonSerializer.SerializeToUtf8Bytes(sortedList);
            cache.Set(key, objectToCache);

            return 1;
        }

        public int AddRange(IEnumerable<T> items) => items.AnySafe() ? items.Sum(Add) : 0;

        public T? FindByHashCode(int[] hashes, IComparer<T>? comparer)
        {
            var items = hashes
                .Select(h =>
                {
                    var objectFromCache = cache.Get(h.ToString());
                    var sortedList = DeserializeObject(objectFromCache);
                    return sortedList.AnySafe() ? sortedList[0] : default;
                })
                .ToList();

            if (comparer != null)
            {
                items.Sort(comparer!);
            }
            else
            {
                items.Sort();
            }

            return items.FirstOrDefault();
        }

        public async Task<T?> FindByHashCodeAsync(int[] hashes, IComparer<T>? comparer, CancellationToken cancellationToken)
        {
            var tasks = hashes
                .Select(async h =>
                {
                    var objectFromCache = await cache.GetAsync(h.ToString());
                    var sortedList = DeserializeObject(objectFromCache);
                    return sortedList.AnySafe() ? sortedList[0] : default;
                })
                .ToList();

            var items = (await Task.WhenAll(tasks)).ToList();

            if (comparer != null)
            {
                items.Sort(comparer!);
            }
            else
            {
                items.Sort();
            }

            return items.FirstOrDefault();
        }

        private static List<T>? DeserializeObject(byte[] buffer)
        {
            if (buffer == null)
            {
                return default;
            }

            var jsonToDeserialize = Encoding.UTF8.GetString(buffer);
            return JsonSerializer.Deserialize<List<T>>(jsonToDeserialize);
        }
    }
}

