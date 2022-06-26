#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Library.Rules
{
    public class Rule1Filters<TFilter1> : BaseRule
    {
        public TFilter1 Filter1 { get; set; }

        public override int GetHashCode() => Generator.GenerateHash(Filter1);
    }
}

