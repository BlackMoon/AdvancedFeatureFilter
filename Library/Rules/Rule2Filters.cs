using System;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Rules
{
    public class Rule2Filters<TFilter1, TFilter2> : Rule1Filters<TFilter1>
    {
        public TFilter2 Filter2 { get; set; }

        public override int GetHashCode() => Generator.GenerateHash(Filter1, Filter2);
    }
}

