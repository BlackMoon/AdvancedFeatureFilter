using Library.Extensions;
using Library.Storage;

namespace AdvancedFeatureFilter.Storage
{
    public class MemStorage<T> : IStorage<T> where T : IComparable<T>
    {
        private readonly IDictionary<int, List<T>> ctx = new Dictionary<int, List<T>>();

        public int Add(T item)
        {
            if (item == null)
            {
                return 0;
            }

            var hash = item.GetHashCode();

            var sortedList = ctx.TryGetValue(hash, out var list) ? list : new List<T>();
            sortedList.AddSorted(item);

            ctx[hash] = sortedList;
            return 1;
        }

        public int AddRange(IEnumerable<T> items) => items.AnySafe() ? items.Sum(Add) : 0;

        public T? FindByHashCode1(int[] hashes, IComparer<T> comparer)
        {
            var items = hashes
                .Select(h => ctx.TryGetValue(h, out var list) && list.Any() ? list[0] : default)
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

        public T? FindByHashCode(int hash) => ctx.TryGetValue(hash, out var list) && list.Any() ? list[0] : default;

        public Task<T?> FindByHashCodeAsync(int hash, CancellationToken cancellationToken) => Task.Run(() => FindByHashCode(hash), cancellationToken);
    }
}

