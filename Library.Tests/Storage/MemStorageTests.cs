using System;
using AdvancedFeatureFilter.Storage;
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
        public void AddsAndItem()
        {
            var rule = new Rule1Filters<int>();
            var total = storage.Add(rule);
            Assert.Equal(1, total);
        }

        [Fact]
        public void AddsRange()
        {
            var rules = new[] { new Rule1Filters<int>(), new Rule1Filters<int>() };
            var total = storage.AddRange(rules);
            Assert.Equal(rules.Length, total);
        }
    }
}

