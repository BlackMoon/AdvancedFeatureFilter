using System;
using Library;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Rules
{
    public class Rule4Filters<TFilter1, TFilter2, TFilter3, TFilter4> : Rule3Filters<TFilter1, TFilter2, TFilter3>
    {
        public TFilter4 Filter4 { get; set; }

        public override int GetHashCode() => HashGenerator.Generate(Filter1, Filter2, Filter3, Filter4);
    }
}
