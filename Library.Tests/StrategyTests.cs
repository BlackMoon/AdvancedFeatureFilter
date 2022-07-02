using System.ComponentModel;
using Library.Rules;
using Library.Storage;
using Library.Strategy;

namespace Library.Tests
{
    public class StrategyTests
    {
        private readonly Rule4Filters<string, string, string, string>[] rules = new[]
            {
                new Rule4Filters<string, string, string, string>() {
                    Filter1 = "AAA",
                    Filter2 = Replacer.AnyString,
                    Filter3 = "CCC",
                    Filter4 = "DDD",
                    Priority = 80,
                    RuleId = 1
                },
                new Rule4Filters<string, string, string, string>() {
                    Filter1 = Replacer.AnyString,
                    Filter2 = Replacer.AnyString,
                    Filter3 = "AAA",
                    Filter4 = Replacer.AnyString,
                    Priority = 1,
                    RuleId = 2
                },
                new Rule4Filters<string, string, string, string>() {
                    Filter1 = "BBB",
                    Filter2 = Replacer.AnyString,
                    Filter3 = "CCC",
                    Filter4 = Replacer.AnyString,
                    Priority = 70,
                    RuleId = 3
                },
                new Rule4Filters<string, string, string, string>() {
                    Filter1 = "AAA",
                    Filter2 = "BBB",
                    Filter3 = "CCC",
                    Filter4 = Replacer.AnyString,
                    Priority = 100,
                    RuleId = 4
                },
                new Rule4Filters<string, string, string, string>() {
                    Filter1 = "CCC",
                    Filter2 = "AAA",
                    Filter3 = Replacer.AnyString,
                    Filter4 = "CCC",
                    Priority = 50,
                    RuleId = 5
                },
                new Rule4Filters<string, string, string, string>()
                {
                    Filter1 = Replacer.AnyString,
                    Filter2 = Replacer.AnyString,
                    Filter3 = Replacer.AnyString,
                    Filter4 = Replacer.AnyString,
                    Priority = 0,
                    RuleId = 6
                }
            };

        private EngineStrategy4<string, string, string, string> strategy;

        public StrategyTests()
        {
            var storage = new MemStorage<Rule4Filters<string, string, string, string>>();
            storage.AddRange(rules);
            strategy = new EngineStrategy4<string, string, string, string>(storage);
        }

        [Theory]
        [InlineData(3, "AAA", "BBB", "CCC", "AAA")]
        [InlineData(3, "AAA", "BBB", "CCC", "DDD")]
        [InlineData(1, "AAA", "AAA", "AAA", "AAA")]
        [InlineData(5, "BBB", "BBB", "BBB", "BBB")]
        [InlineData(2, "BBB", "CCC", "CCC", "CCC")]
        [Description("If 2 or more rules have the same hashcode, return the rule with the highest priority")]
        public void FindRule(int ruleIx, string val1, string val2, string val3, string val4)
        {
            var result = strategy.FindRule(val1, val2, val3, val4);
            Assert.Equal(rules[ruleIx], result);
        }

        [Fact]
        public async Task LoadRules()
        {
            var validPath = "sample_rules.csv";
            var ex = await Record.ExceptionAsync(() => strategy.LoadRules(validPath));
            Assert.Null(ex);

            var invalidPath = "invalid_path.csv";
            await Assert.ThrowsAsync<FileNotFoundException>(() => strategy.LoadRules(invalidPath));
        }
    }
}

