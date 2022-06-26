namespace Library.Tests;

public class GeneratorTests
{
    [Fact]
    public void AllCombinations()
    {
        var expected = new[] {
            new[] { 1 },
            new[] { 2 },
            new[] { 3 },
            new[] { 1, 2 },
            new[] { 1, 3 },
            new[] { 2, 3 },
            new[] { 1, 2, 3 } }
        .ToArray();

        var result = Generator.AllCombinations(3);

        Assert.Equal(expected.Length, result.Count());

        var i = 0;
        foreach (var row in result)
        { 
            Assert.True(expected[i].SequenceEqual(row));
            i++;
        }
    }

    [Fact]
    public void Combinations()
    {
        var expected = new[] { new [] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 } }.ToArray();
        var result = Generator.Combinations(2, 3);

        Assert.Equal(expected.Length, result.Count());

        var i = 0;
        foreach (var row in result)
        {
            Assert.True(expected[i].SequenceEqual(row));
            i++;
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(392, 1)]
    [InlineData(9019, 1, 2)]
    [InlineData(207442, 1, 2, 3)]
    [InlineData(4771173, 1, 2, 3, 4)]
    public void GenerateHash(int expected, params object[] args)
    {
        var result = Generator.GenerateHash(args);
        Assert.Equal(expected, result);

        result = Generator.GenerateHash(DateTime.Now);
        Assert.NotEqual(0, result);

        result = Generator.GenerateHash("string");
        Assert.NotEqual(0, result);
    }

    [Fact]
    public void ShouldReturn_TheSameHash_ForTheSameObject()
    {
        var result1 = Generator.GenerateHash("string");
        var result2 = Generator.GenerateHash("string");
        Assert.Equal(result1, result2);
    }

   
}