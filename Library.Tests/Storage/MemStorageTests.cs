﻿using AdvancedFeatureFilter.Storage;
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
        public void AddsItem()
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

        [Fact]
        public void FindByHashCode()
        {
            var hashes = new[] { 1000 };
            var ex = Record.Exception(() => storage.FindByHashCode(hashes));
            Assert.Null(ex);
        }

        [Fact]
        public async Task FindByHashCodeAsync()
        {
            var hashes = new[] { 1000 };
            var ex = await Record.ExceptionAsync(() => storage.FindByHashCodeAsync(hashes));
            Assert.Null(ex);
        }
    }
}

