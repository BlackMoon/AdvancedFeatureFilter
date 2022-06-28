using BenchmarkDotNet.Attributes;
using Library.Rules;
using Library.Storage;
using Library.Strategy;

#pragma warning disable CS8604 // Possible null reference argument.

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

        [Params("AAA", "AAA")]
        public string? Val1 { get; set; }

        [Params("BBB", "BBB")]
        public string? Val2 { get; set; }

        [Params("CCC", "CCC")]
        public string? Val3 { get; set; }

        [Params("AAA", "DDD")]
        public string? Val4 { get; set; }

        [Benchmark]
        public void FindRule()
        {
            try
            {
                var rule = strategy.FindRule(Val1, Val2, Val3, Val4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

