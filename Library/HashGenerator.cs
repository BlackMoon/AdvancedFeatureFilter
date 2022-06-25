using System;
using Library.Extensions;

namespace Library
{
    public static class HashGenerator
    {
        public static int Generate(params object[] args)
        {
            if (!args.AnySafe())
            {
                return 0;
            }

            unchecked
            {
                var hash = 17;
                for (var i = 0; i < args.Length; i++)
                {
                    hash = hash * 23 + (i + args[i]?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }
    }
}

