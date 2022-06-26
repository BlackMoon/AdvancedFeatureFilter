using Library.Rules;
using Library.Storage;

#pragma warning disable CS8601 // Possible null reference assignment.

namespace Library.Strategy
{
    public class EngineStrategy3<TFilter1, TFilter2, TFilter3> : CombinationBasedStrategy, IStrategy3<TFilter1, TFilter2, TFilter3>
    {
        private readonly IStorage<Rule3Filters<TFilter1, TFilter2, TFilter3>> storage;

        public EngineStrategy3(IStorage<Rule3Filters<TFilter1, TFilter2, TFilter3>> storage)
        {
            Combinations = Generator.AllCombinations(3);
            this.storage = storage;
        }

        public Rule3Filters<TFilter1, TFilter2, TFilter3>? FindRule(TFilter1 val1, TFilter2 val2, TFilter3 val3)
        {
            var hashes = GetAllHashes(new object[] { val1, val2, val3 }).ToArray();
            return storage.FindByHashCode(hashes);
        }

        public async Task LoadRules(string path, char separator = ',')
        {
            using var fs = File.OpenRead(path);
            await foreach (var rule in DataReader.ReadFromStreamAsync<Rule3Filters<TFilter1, TFilter2, TFilter3>>(
                fs,
                new DataReaderOptions { Separator = separator }))
            {
                storage.Add(rule);
            }
        }
    }
}
