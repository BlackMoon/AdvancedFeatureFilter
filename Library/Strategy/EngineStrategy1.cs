using Library.Rules;
using Library.Storage;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Strategy
{
    public class EngineStrategy1<TFilter1> : IStrategy1<TFilter1>
    {
        private readonly IStorage<Rule1Filters<TFilter1>> storage;

        public EngineStrategy1(IStorage<Rule1Filters<TFilter1>> storage)
        {
            this.storage = storage;
        }

        public Rule1Filters<TFilter1>? FindRule(TFilter1 val1)
        {
            var hashes = new[] {
                Generator.GenerateHash(val1),
                Generator.GenerateHash(Replacer.ReplaceWithAny(val1))
            };

            return storage.FindByHashCode(hashes);
        }

        public async Task LoadRules(string path, char separator = ',')
        {
            using var fs = File.OpenRead(path);
            await foreach (var rule in DataReader.ReadFromStreamAsync<Rule1Filters<TFilter1>>(
                fs,
                new DataReaderOptions { Separator = separator }))
            {
                storage.Add(rule);
            }
        }
    }
}
