using Library.Rules;
using Library.Storage;

#pragma warning disable CS8601 // Possible null reference assignment.

namespace Library.Strategy
{
    public class EngineStrategy2<TFilter1, TFilter2> : CombinationBasedStrategy, IStrategy2<TFilter1, TFilter2>
    {
        private readonly IStorage<Rule2Filters<TFilter1, TFilter2>> storage;

        public EngineStrategy2(IStorage<Rule2Filters<TFilter1, TFilter2>> storage)
        {
            this.Combinations = Generator.AllCombinations(2);
            this.storage = storage;
        }

        public Rule2Filters<TFilter1, TFilter2>? FindRule(TFilter1 val1, TFilter2 val2)
        {
            var hashes = GetAllHashes(new object[] { val1, val2 }).ToArray();
            return storage.FindByHashCode(hashes);
        }

        public async Task LoadRules(string path, char separator = ',')
        {
            using var fs = File.OpenRead(path);
            await foreach (var rule in DataReader.ReadFromStreamAsync<Rule2Filters<TFilter1, TFilter2>>(
                fs,
                new DataReaderOptions { Separator = separator }))
            {
                storage.Add(rule);
            }
        }
    }
}
