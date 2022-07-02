namespace Library.Storage
{
    public interface IStorage<T> 
    {
        int Add(T item);

        int AddRange(IEnumerable<T> items);

        T? FindByHashCode(int[] hashes, IComparer<T>? comparer = null);

        Task<T?> FindByHashCodeAsync(int[] hashes, IComparer<T>? comparer = null, CancellationToken cancellationToken = default);
    }
}

