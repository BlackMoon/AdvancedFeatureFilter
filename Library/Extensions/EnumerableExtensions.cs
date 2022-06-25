using System;

namespace Library.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool AnySafe<T>(this IEnumerable<T> source) => source?.Any() ?? false;
    }
}

