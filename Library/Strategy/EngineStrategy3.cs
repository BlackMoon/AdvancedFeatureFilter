using System;
using Library.Rules;
using Library.Storage;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Strategy
{
    public class EngineStrategy3<TFilter1, TFilter2, TFilter3> : IStrategy3<TFilter1, TFilter2, TFilter3>
    {
        private readonly IStorage<Rule3Filters<TFilter1, TFilter2, TFilter3>> storage;

        public EngineStrategy3(IStorage<Rule3Filters<TFilter1, TFilter2, TFilter3>> storage)
        {
            this.storage = storage;
        }

        public Rule3Filters<TFilter1, TFilter2, TFilter3>? FindRule(TFilter1 val1, TFilter2 val2, TFilter3 val3)
        {
            var hash = Generator.GenerateHash(val1, val2, val3);
            return storage.FindByHashCode(hash);
        }
    }
}
