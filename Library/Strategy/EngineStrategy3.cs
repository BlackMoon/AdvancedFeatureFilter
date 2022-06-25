using System;
using Library.Rules;
using Library.Storage;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Strategy
{
    public class EngineStrategy4<TFilter1, TFilter2, TFilter3, TFilter4> : IStrategy4<TFilter1, TFilter2, TFilter3, TFilter4>
    {
        private readonly IStorage<Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>> storage;

        public EngineStrategy4(IStorage<Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>> storage)
        {
            this.storage = storage;
        }

        public Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4>? FindRule(TFilter1 val1, TFilter2 val2, TFilter3 val3, TFilter4 val4)
        {
            var hash = HashGenerator.Generate(val1, val2, val3, val4);
            return storage.FindByHashCode(hash);
        }
    }
}
