using Library.Extensions;
using Library.Storage;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

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

        public T? FindByHashCode(int[] hashes, IComparer<T>? comparer)
        {
            var items = hashes
                .Select(h => (ctx.TryGetValue(h, out var list), list))
                .Where(t => t.Item1)
                .Select(t => t.Item2[0])
                .ToList();

            if (comparer != null)
            {
                items.Sort(comparer);
            }
            else
            {
                items.Sort();
            }

            return items.FirstOrDefault();
        }

        public Task<T?> FindByHashCodeAsync(int[] hashes, IComparer<T>? comparer, CancellationToken cancellationToken) =>
            Task.Run(() => FindByHashCode(hashes, comparer), cancellationToken);
    }
}

