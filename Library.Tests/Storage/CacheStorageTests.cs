using Library.Rules;
using Library.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Library.Tests.Storage
{
    public class CacheStorageTests
    {
        private Mock<IDistributedCache> cacheMock;
        private IStorage<Rule1Filters<int>> storage;

        public CacheStorageTests()
        {
            cacheMock = new Mock<IDistributedCache>();

            cacheMock
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(It.IsAny<byte[]>);

            cacheMock
                .Setup(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(It.IsAny<byte[]>);

            storage = new CacheStorage<Rule1Filters<int>>(cacheMock.Object);
        }

        [Fact]
        public void Add()
        {
            var rule = new Rule1Filters<int>();
            var key = rule.GetHashCode().ToString();

            var total = storage.Add(rule);
            Assert.Equal(1, total);

            cacheMock.Verify(c => c.Get(key), Times.Once);
        }

        [Fact]
        public void AddRange()
        {
            var rules = new[] {
                new Rule1Filters<int>() { Filter1 = 1 },
                new Rule1Filters<int>() { Filter1 = 2 }
            };

            var total = storage.AddRange(rules);
            Assert.Equal(rules.Length, total);

            foreach (var r in rules)
            {
                cacheMock.Verify(c => c.Get(r.GetHashCode().ToString()), Times.Once);
            }
        }

        [Fact]
        public void FindByHashCode()
        {
            var hashes = new[] { 1000, 2000 };
            var ex = Record.Exception(() => storage.FindByHashCode(hashes));
            Assert.Null(ex);

            foreach (var hash in hashes)
            {
                cacheMock.Verify(c => c.Get(hash.ToString()), Times.Once);
            }
        }

        [Fact]
        public async Task FindByHashCodeAsync()
        {
            var hashes = new[] { 1000, 2000 };
            var ex = await Record.ExceptionAsync(() => storage.FindByHashCodeAsync(hashes));
            Assert.Null(ex);

            foreach (var hash in hashes)
            {
                cacheMock.Verify(c => c.GetAsync(hash.ToString(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}

