using System;

namespace Library
{
    public static class HashGenerator
    {
        public static int Generate(params object[] args)
        {
            if (args == null)
            {
                return 0;
            }

            const int seed = 17;

            unchecked
            {
                return args.Aggregate(seed, (total, next) => total = total * 23 + next?.GetHashCode() ?? 0);
            }
        }
    }
}

