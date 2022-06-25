using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Rules
{
    [Table("rules")]
    public abstract class BaseRule : IComparable<BaseRule>
    {
        public int RuleId { get; set; }

        public int Priority { get; set; }

        public int? OutputValue { get; set; }

        public int CompareTo(BaseRule? other)
        {
            return other == null ? 1 : Priority.CompareTo(other.Priority);
        }
    }
}

