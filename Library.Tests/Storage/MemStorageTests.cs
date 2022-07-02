using System.ComponentModel;
using Library.Rules;
using Library.Storage;

namespace Library.Tests.Storage
{
    public class MemStorageTests
    {
        private IStorage<Rule1Filters<int>> storage;

        public MemStorageTests()
        {
            storage = new MemStorage<Rule1Filters<int>>();
        }

        [Fact]
        public void Add()
        {
            var rule = new Rule1Filters<int>();
            var total = storage.Add(rule);
            Assert.Equal(1, total);
        }

        [Fact]
        public void AddRange()
        {
            var rules = new[] {
                new Rule1Filters<int>() { Filter1 = 1 },
                new Rule1Filters<int>() { Filter1 = 2 }
            };

            var total = storage.AddRange(rules);
            Assert.Equal(rules.Length, total);
        }

        [Fact]
        [Description("If 2 or more rules have the same hashcode, return the rule with the highest priority")]
        public void FindByHashCode()
        {
            var rules = new[] {
                new Rule1Filters<int>() { Filter1 = 1, Priority = 5 },
                new Rule1Filters<int>() { Filter1 = 2, Priority = 10 }
            };

            storage.AddRange(rules);

            var hashes = rules.Select(r => r.GetHashCode()).ToArray();
            var result = storage.FindByHashCode(hashes);
            
            Assert.Equal(result, rules[1]);
        }

        [Fact]
        public async Task FindByHashCodeAsync()
        {
            var rules = new[] {
                new Rule1Filters<int>() { Filter1 = 1, Priority = 5 },
                new Rule1Filters<int>() { Filter1 = 2, Priority = 10 }
            };

            storage.AddRange(rules);

            var hashes = rules.Select(r => r.GetHashCode()).ToArray();
            var result = await storage.FindByHashCodeAsync(hashes);

            Assert.Equal(result, rules[1]);
        }
    }
}

