using AdvancedFeatureFilter;
using Library;
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
        services.AddStrategy(typeof(string), typeof(string), typeof(string), typeof(string));
    })
    .Build();

var services = host.Services;
var appConfig = services.GetRequiredService<IOptions<AppConfig>>();
var strategy = services.GetRequiredService<IStrategy4<string, string, string, string>>();
try
{
    await strategy.LoadRules(appConfig.Value.CsvFile!);

    var rule = strategy.FindRule("BBB", "CCC", "CCC", "CCC");
    Console.WriteLine(rule);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
