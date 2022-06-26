
using Library.Extensions;
using Library.Rules;

namespace Library.Tests
{
    public class DataReaderTests
    {
        [Fact]
        public async Task EmptyText()
        {
            await CheckOutput(new string[0], Enumerable.Empty<string[]>(), Enumerable.Empty<Rule1Filters<string>>());
        }

        [Fact]
        public async Task HeaderOnly()
        {
            await CheckOutput(new[] { "A", "B", "C" }, Enumerable.Empty<string[]>(), Enumerable.Empty<Rule1Filters<string>>());
        }

        [Fact]
        public async Task HeaderAndRows()
        {
            await CheckOutput(
                new [] { "RuleId", "Priority", "OutputValue", "Filter1" },
                new[] {
                    new[] { "1", "1", "1", Replacer.AnyString },
                    new[] { "2", "2", "2", Replacer.AnyString },
                    new[] { "3", "3", "3", Replacer.AnyString }
                },
                new[] {
                    new Rule1Filters<string> { RuleId = 1, Priority = 1, OutputValue = 1, Filter1 = Replacer.AnyString },
                    new Rule1Filters<string> { RuleId = 2, Priority = 2, OutputValue = 2, Filter1 = Replacer.AnyString },
                    new Rule1Filters<string> { RuleId = 3, Priority = 3, OutputValue = 3, Filter1 = Replacer.AnyString }
                    }
                );
        }

        [Fact]
        public async Task HeaderAndRows_Replacement()
        {
            await CheckOutput(
                new[] { "RuleId", "Priority", "OutputValue", "Filter1" },
                new[] {
                    new[] { "1", "1", "1", "10" },
                    new[] { "2", "2", "2", Replacer.AnyString },
                    new[] { "3", "3", "3", Replacer.AnyString }
                },
                new[] {
                    new Rule1Filters<int> { RuleId = 1, Priority = 1, OutputValue = 1, Filter1 = 10 },
                    new Rule1Filters<int> { RuleId = 2, Priority = 2, OutputValue = 2, Filter1 = Replacer.AnyInt },
                    new Rule1Filters<int> { RuleId = 3, Priority = 3, OutputValue = 3, Filter1 = Replacer.AnyInt }
                    }
                );
        }

        [Fact]
        public async Task HeaderAndRowsWithNotEnoughColumns()
        {
            await CheckOutput(
                new[] { "RuleId", "Priority", "OutputValue", "Filter1" },
                new[] {
                    new[] { "1", "1", "1", Replacer.AnyString },
                    new[] { "2", "2", "2" },
                    new[] { "3", "3" }
                },
                new[] {
                    new Rule1Filters<string> { RuleId = 1, Priority = 1, OutputValue = 1, Filter1 = Replacer.AnyString },
                    new Rule1Filters<string> { RuleId = 2, Priority = 2, OutputValue = 2 },
                    new Rule1Filters<string> { RuleId = 3, Priority = 3 }
                    }
                );
        }

        [Fact]
        public async Task FailedConversion()
        {
            var ex = await Assert.ThrowsAsync<InvalidCastException>(() => CheckOutput(new[] { "RuleId", "Priority", "OutputValue", "Filter1" },
                new[] {
                    new[] { "1", "1", Replacer.AnyString },
                },
                new[] {
                    new Rule1Filters<string> { RuleId = 1, Priority = 1, OutputValue = 1, Filter1 = Replacer.AnyString },
                    }
                )
            );

            Assert.Equal("Failed to convert value(1, 3) to OutputValue", ex.Message);
        }

        private static async Task CheckOutput<T>(string[] headers, IEnumerable<string[]> lines, IEnumerable<T> expectedItems, char separator = ',') where T : new()
        {
            var rows = lines.ToArray();

            using var writer = new StringWriter();
            await writer.WriteLineAsync(string.Join(separator, headers));

            foreach (var row in rows)
            {
                await writer.WriteLineAsync(string.Join(separator, row));
            }

            var result = writer.ToString();
            
            var items = await DataReader.ReadFromTextAsync<T>(result).ToListAsync();

            Assert.Equal(rows.Length, items.Count);
            Assert.Equal(expectedItems, items);

            if (items.Any())
            {
                var props = items[0]!.GetType().GetProperties();
                foreach (var p in props)
                {
                    Assert.True(headers.Contains(p.Name), "item[0].Properties.SequenceEqual(headers)");
                }
            }
        }
    }
}

