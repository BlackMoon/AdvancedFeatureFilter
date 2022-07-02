
using BenchmarkDotNet.Attributes;
using Library.Rules;
using Library.Storage;
using Library.Strategy;

namespace Library.Benchmark
{
    [MemoryDiagnoser]
    public class LoadRulesBenchmark
    {
        private readonly IStorage<Rule4Filters<string, string, string, string>> storage;
        private readonly IStrategy4<string, string, string, string> strategy;

        public LoadRulesBenchmark()
        {
            storage = new MemStorage<Rule4Filters<string, string, string, string>>();
            strategy = new EngineStrategy4<string, string, string, string>(storage);
        }

        [Benchmark]
        public void LoadRules()
        {
            try
            {
                strategy.LoadRules("sample_rules.csv").Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

