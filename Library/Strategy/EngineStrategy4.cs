using System;
using Library.Extensions;
using Library.Rules;
using Library.Storage;

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Strategy
{
    public class EngineStrategy4<TFilter1, TFilter2, TFilter3, TFilter4> : IStrategy4<TFilter1, TFilter2, TFilter3, TFilter4>
    {
        private readonly IEnumerable<int[]> combinations;
        private readonly IStorage<Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>> storage;

        public EngineStrategy4(IStorage<Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>> storage)
        {
            this.combinations = Generator.AllCombinations(4);
            this.storage = storage;
        }

        public Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>? FindRule(TFilter1 val1, TFilter2 val2, TFilter3 val3, TFilter4 val4)
        {
           
            var hashes = GetAllHashes(new object[] { val1, val2, val3, val4 }).ToArray();

           

            var hash = Generator.GenerateHash(val1, val2, val3, val4);
            return storage.FindByHashCode(hash);
        }

        private IEnumerable<int> GetAllHashes(object [] filters)
        {
            if (!filters.AnySafe())
            {
                yield break;
            }

            foreach (var row in combinations)
            {
                var modifiedFilters = new object[filters.Length];
                for (var i = 0; i < filters.Length; i++)
                {
                    modifiedFilters[i] = row.Any(r => i == r - 1) ? filters[i] : Replacer.ReplaceWithAny(filters[i]);
                }

                yield return Generator.GenerateHash(modifiedFilters);
            }
        }
    }
}
