using AdvancedFeatureFilter;
using AdvancedFeatureFilter.Extensions;
using AdvancedFeatureFilter.Generator;
using AdvancedFeatureFilter.Rules;
using AdvancedFeatureFilter.Storage;
using AdvancedFeatureFilter.Strategy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .Build();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddOptions<AppConfig>().Bind(config);
        services.AddStrategy(new[] { typeof(string), typeof(string), typeof(string) });
    })
    .Build();

var services = host.Services;
var appConfig = services.GetRequiredService<IOptions<AppConfig>>();
var storage = services.GetRequiredService<IStorage<Rule3Filters<string, string, string>>>();
var strategy = services.GetRequiredService<IStrategy3<string, string, string>>();
try
{
    var csv = File.ReadAllText(appConfig.Value.CsvFile!);
    var rules = await DataReader.ReadFromTextAsync<Rule3Filters<string, string, string>>(csv).ToListAsync();
    storage.AddRange(rules);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

var rule = strategy.FindRule("1", "2", "3");

