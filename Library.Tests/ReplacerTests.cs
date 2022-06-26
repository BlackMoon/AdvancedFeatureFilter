namespace Library.Tests
{
    public class ReplacerTests
    {
        [Theory]
        [InlineData(1, Replacer.AnyInt)]
        [InlineData(1L, Replacer.AnyInt)]
        [InlineData(1f, Replacer.AnyInt)]
        [InlineData(1d, Replacer.AnyInt)]
        [InlineData(true, Replacer.AnyBool)]
        [InlineData("string", Replacer.AnyString)]
        public void ReplaceValues(object arg, object expected)
        {
            var result = Replacer.ReplaceWithAny(arg);
            Assert.Equal(expected, result);

            result = Replacer.ReplaceWithAny(1m);
            Assert.Equal(Replacer.AnyInt, result);

            result = Replacer.ReplaceWithAny(DateTime.Now);
            Assert.Equal(Replacer.AnyDate, result);
        }

        [Theory]
        [InlineData(typeof(int), Replacer.AnyInt)]
        [InlineData(typeof(long), Replacer.AnyInt)]
        [InlineData(typeof(decimal), Replacer.AnyInt)]
        [InlineData(typeof(double), Replacer.AnyInt)]
        [InlineData(typeof(float), Replacer.AnyInt)]
        [InlineData(typeof(bool), Replacer.AnyBool)]
        [InlineData(typeof(string), Replacer.AnyString)]
        public void ReplaceValues_ForType(Type arg, object expected)
        {
            var result = Replacer.ReplaceWithAny(arg);
            Assert.Equal(expected, result);

            result = Replacer.ReplaceWithAny(typeof(DateTime));
            Assert.Equal(Replacer.AnyDate, result);
        }
    }
}

