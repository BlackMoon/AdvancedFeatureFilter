using Library.Rules;
using Library.Storage;

#pragma warning disable CS8601 // Possible null reference assignment.

namespace Library.Strategy
{
    public class EngineStrategy4<TFilter1, TFilter2, TFilter3, TFilter4> : CombinationBasedStrategy, IStrategy4<TFilter1, TFilter2, TFilter3, TFilter4>
    {
        private readonly IStorage<Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>> storage;

        public EngineStrategy4(IStorage<Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>> storage)
        {
            Combinations = Generator.AllCombinations(4).ToArray();
            this.storage = storage;
        }

        public Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>? FindRule(TFilter1 val1, TFilter2 val2, TFilter3 val3, TFilter4 val4)
        {
            var hashes = GetAllHashes(new object[] { val1, val2, val3, val4 }).ToArray();
            return storage.FindByHashCode(hashes);
        }

        public async Task LoadRules(string path, char separator = ',')
        {
            using var fs = File.OpenRead(path);
            await foreach (var rule in DataReader.ReadFromStreamAsync<Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>>(
                fs,
                new DataReaderOptions { Separator = separator }))
            {
                storage.Add(rule);
            }  
        }
    }
}
