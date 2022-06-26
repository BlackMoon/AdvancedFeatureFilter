#pragma warning disable CS8604 // Possible null reference argument.

namespace Library
{
    public static class Replacer
    {
        public const int AnyInt = -99;
        public const bool AnyBool = false;
        public const string AnyString = "<ANY>";
        public static DateTime AnyDate = new DateTime(1, 1, 1);

        private static readonly IDictionary<Type, object> AnyTypes = new Dictionary<Type, object>() {
            { typeof(int), AnyInt },
            { typeof(long), AnyInt },
            { typeof(decimal), AnyInt },
            { typeof(double), AnyInt },
            { typeof(float), AnyInt },
            { typeof(bool), AnyBool },
            { typeof(DateTime), AnyDate },
            { typeof(string), AnyString }
        };

        public static object ReplaceWithAny<T>(T value) => ReplaceWithAny(value?.GetType());

        public static object ReplaceWithAny(Type conversionType) =>
            AnyTypes.TryGetValue(conversionType, out var anyValue) ? anyValue : AnyString;
    }
}

