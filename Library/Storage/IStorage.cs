using System;

namespace Library.Storage
{
    public interface IStorage<T> 
    {
        int Add(T item);

        int AddRange(IEnumerable<T> items);

        T? FindByHashCode(int hash);

        Task<T?> FindByHashCodeAsync(int hash, CancellationToken cancellationToken);
    }
}

