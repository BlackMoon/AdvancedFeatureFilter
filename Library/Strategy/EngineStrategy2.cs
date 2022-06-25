using System;
using Library.Rules;
using Library.Storage;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Strategy
{
    public class EngineStrategy2<TFilter1, TFilter2> : IStrategy2<TFilter1, TFilter2>
    {
        private readonly IStorage<Rule2Filters<TFilter1, TFilter2>> storage;

        public EngineStrategy2(IStorage<Rule2Filters<TFilter1, TFilter2>> storage)
        {
            this.storage = storage;
        }

        public Rule2Filters<TFilter1, TFilter2>? FindRule(TFilter1 val1, TFilter2 val2)
        {
            var hash = HashGenerator.Generate(val1, val2);
            return storage.FindByHashCode(hash);
        }
    }
}
