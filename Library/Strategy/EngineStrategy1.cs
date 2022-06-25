using System;
using Library.Rules;
using Library.Storage;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Strategy
{
    public class EngineStrategy1<TFilter1> : IStrategy1<TFilter1>
    {
        private readonly IStorage<Rule1Filters<TFilter1>> storage;

        public EngineStrategy1(IStorage<Rule1Filters<TFilter1>> storage)
        {
            this.storage = storage;
        }

        public Rule1Filters<TFilter1>? FindRule(TFilter1 val1)
        {
            var hash = HashGenerator.Generate(val1);
            return storage.FindByHashCode(hash);
        }
    }
}
