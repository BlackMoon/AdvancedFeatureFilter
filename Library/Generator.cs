using Library.Extensions;

namespace Library
{
    public static class Generator
    {
        /// <summary>
        /// All combinations without repetition from 1 to n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<int[]> AllCombinations(int n) => Enumerable.Range(1, n).SelectMany(i => Combinations(i, n));

        /// <summary>
        /// Combinations without repetition k-th class of n element
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<int[]> Combinations(int k, int n)
        {
            var result = new int[k];
            var stack = new Stack<int>();
            stack.Push(1);

            while (stack.Count > 0)
            {
                var index = stack.Count - 1;
                var value = stack.Pop();

                while (value <= n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index == k)
                    {
                        yield return (int[])result.Clone();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Hash of an array of elements given the position of the element
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int GenerateHash(params object[] args)
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
                    var argHash = i;
                    if (args[i] != null)
                    {
                        argHash += args[i] is string ? ((string)args[i]).GetDeterministicHashCode() : args[i].GetHashCode();
                    }
                    hash = hash * 23 + argHash;
                }

                return hash;
            }
        }
    }
}

