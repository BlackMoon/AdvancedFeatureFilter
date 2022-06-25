namespace Library.Tests;

public class GeneratorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(392, 1)]
    [InlineData(9019, 1, 2)]
    [InlineData(207442, 1, 2, 3)]
    [InlineData(4771173, 1, 2, 3, 4)]
    public void GenerateHashForInt(int expected, params object[] args)
    {
        var result = HashGenerator.Generate(args);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GenerateHashForDate()
    {
        var result = HashGenerator.Generate(DateTime.Now);
        Assert.NotEqual(0, result);
    }

    [Fact]
    public void GenerateHashForString()
    {
        var result = HashGenerator.Generate("string");
        Assert.NotEqual(0, result);
    }
}