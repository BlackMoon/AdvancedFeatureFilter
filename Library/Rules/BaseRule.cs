using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Rules
{
    [Table("rules")]
    public abstract class BaseRule : IComparable<BaseRule>
    {
        public int RuleId { get; set; }

        public int Priority { get; set; }

        public int? OutputValue { get; set; }

        // sort by Priority desc
        public int CompareTo(BaseRule? other) => other == null ? 1 : other.Priority.CompareTo(Priority);
    }
}

