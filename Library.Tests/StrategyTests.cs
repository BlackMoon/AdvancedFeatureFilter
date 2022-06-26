using Library.Rules;
using Library.Storage;
using Library.Strategy;
using Moq;

namespace Library.Tests
{
    public class StrategyTests
    {
        private Mock<IStorage<Rule4Filters<string, string, string, string>>> storageMock;
        private EngineStrategy4<string, string, string, string> strategy;

        public StrategyTests()
        {
            storageMock = new Mock<IStorage<Rule4Filters<string, string, string, string>>>();

            storageMock
                .Setup(c => c.FindByHashCode(It.IsAny<int[]>(), It.IsAny<IComparer<Rule4Filters<string, string, string, string>>>()))
                .Returns(It.IsAny<Rule4Filters<string, string, string, string>>());

            strategy = new EngineStrategy4<string, string, string, string>(storageMock.Object);
        }

        [Fact]
        public void FindRule()
        {
            var val1 = Replacer.AnyString;
            var val2 = Replacer.AnyString;
            var val3 = Replacer.AnyString;
            var val4 = Replacer.AnyString;

            strategy.FindRule(val1, val2, val3, val4);

            var hashes = new[] { Generator.GenerateHash(val3, val2, val3, val4) };
            storageMock.Verify(c => c.FindByHashCode(hashes, It.IsAny<IComparer<Rule4Filters<string, string, string, string>>>()), Times.Once);
        }

        [Fact]
        public async Task LoadRules()
        {
            var validPath = "sample_rules.csv";
            var ex = await Record.ExceptionAsync(() => strategy.LoadRules(validPath));
            Assert.Null(ex);

            var invalidPath = "invalid_path.csv";
            await Assert.ThrowsAsync<FileNotFoundException>(() => strategy.LoadRules(invalidPath));
        }
    }
}

