using System;
namespace Library.Rules
{
    public abstract class BaseRule
    {
        public int RuleId { get; set; }

        public int Priority { get; set; }

        public int? OutputValue { get; set; }
    }
}

