using AdvancedFeatureFilter;
using Library;
using Library.Extensions;
using Library.Rules;
using Library.Storage;
using Library.Strategy;
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
        services.AddDistributedMemoryCache();
        services.AddOptions<AppConfig>().Bind(config);
        services.AddStrategy(typeof(string), typeof(string), typeof(string));
    })
    .Build();

var services = host.Services;
var appConfig = services.GetRequiredService<IOptions<AppConfig>>();
var storage = services.GetRequiredService<IStorage<Rule3Filters<string, string, string>>>();
var strategy = services.GetRequiredService<IStrategy3<string, string, string>>();
try
{
    //var csv = File.ReadAllText(appConfig.Value.CsvFile!);
    var csv = "ruleId, priority, outputValue, filter1\r\n1,2,3,4\r\n1,2,3";
    await foreach (var rule in DataReader.ReadFromTextAsync<Rule3Filters<string, string, string>>(csv))
    {
        storage.Add(rule);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

var rule1 = strategy.FindRule("4", null, null);

Console.WriteLine(rule1);