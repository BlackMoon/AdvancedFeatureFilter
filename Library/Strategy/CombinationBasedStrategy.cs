using Library.Extensions;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Strategy
{
    public abstract class CombinationBasedStrategy
    {
        public IEnumerable<int[]>? Combinations { get; protected set; }

        protected IEnumerable<int> GetAllHashes(object[] values)
        {
            if (!Combinations.AnySafe() || !values.AnySafe())
            {
                yield break;
            }

            int hash;
            var proccessed = new HashSet<int>();

            // Replace values with <ANY> according to combinations
            foreach (var row in Combinations)
            {
                var filters = values
                    .Select((v, i) => row.Any(r => i == r - 1) ? v : Replacer.ReplaceWithAny(v))
                    .ToArray();

                hash = Generator.GenerateHash(filters);
                if (!proccessed.Contains(hash))
                {
                    proccessed.Add(hash);
                    yield return hash;
                }
            }

            // last row - Replace all values with <ANY>
            var anyValueFilters = values
                .Select((v, i) => Replacer.ReplaceWithAny(v))
                .ToArray();

            hash = Generator.GenerateHash(anyValueFilters);
            if (!proccessed.Contains(hash))
            {
                proccessed.Add(hash);
                yield return hash;
            }
        }
    }
}

