using System;
namespace Library.Extensions
{
    public static class SpanExtensions
    {
        public static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, T separator, StringSplitOptions options = StringSplitOptions.None)
            where T : IEquatable<T>
        {
            if (!Enum.IsDefined(typeof(StringSplitOptions), options))
            {
                throw new ArgumentException($"Invalid value for {nameof(options)}: {options}");
            }

            return new SpanSplitEnumerator<T>(span, separator, options == StringSplitOptions.RemoveEmptyEntries);
        }

        public ref struct SpanSplitEnumerator<T> where T : IEquatable<T>
        {
            private readonly T sep;
            private ReadOnlySpan<T> sequence;
            private SpanSplitInfo spanSplitInfo;

            private bool ShouldRemoveEmptyEntries => spanSplitInfo.HasFlag(SpanSplitInfo.RemoveEmptyEntries);
            private bool IsFinished => spanSplitInfo.HasFlag(SpanSplitInfo.FinishedEnumeration);

            public ReadOnlySpan<T> Current { get; private set; }
            public SpanSplitEnumerator<T> GetEnumerator() => this;

            internal SpanSplitEnumerator(ReadOnlySpan<T> span, T separator, bool removeEmptyEntries)
            {
                Current = default;
                
                sep = separator;
                sequence = span;
                spanSplitInfo = default(SpanSplitInfo) | (removeEmptyEntries ? SpanSplitInfo.RemoveEmptyEntries : 0);
            }

            public bool MoveNext()
            {
                if (IsFinished) {
                    return false;
                }

                do
                {
                    var index = sequence.IndexOf(sep);

                    if (index < 0)
                    {
                        Current = sequence;
                        spanSplitInfo |= SpanSplitInfo.FinishedEnumeration;
                        return !(ShouldRemoveEmptyEntries && Current.IsEmpty);
                    }

                    Current = sequence[..index];
                    sequence = sequence[(index + 1)..];
                }
                while (Current.IsEmpty && ShouldRemoveEmptyEntries);

                return true;
            }

            [Flags]
            private enum SpanSplitInfo : byte
            {
                RemoveEmptyEntries = 0x1,
                FinishedEnumeration = 0x2
            }
        }

    }
}

