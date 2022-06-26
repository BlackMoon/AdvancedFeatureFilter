using BenchmarkDotNet.Attributes;
using Library.Rules;
using Library.Storage;
using Library.Strategy;

namespace Library.Benchmark
{
    public class FindRuleBenchmark
    {
        private readonly IStorage<Rule4Filters<string, string, string, string>> storage;
        private readonly IStrategy4<string, string, string, string> strategy;

        public FindRuleBenchmark()
        {
            storage = new MemStorage<Rule4Filters<string, string, string, string>>();
            strategy = new EngineStrategy4<string, string, string, string>(storage);
            try
            {
                strategy.LoadRules("sample_rules.csv").Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [Benchmark]
        public void FindRule()
        {
            try
            {
                strategy.FindRule("BBB", "CCC", "CCC", "CCC");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

