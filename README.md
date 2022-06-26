# AdvancedFeatureFilter
Find best matching rules for datasets with filtering values

## Features
- Load data from a file to array of strongly typed entities. 

For example.

```cs
class StrategyRule 
{
  public int RuleId;
  public int Priority;
  public string Filter1;
  public string Filter2;
  public string Filter3;
  public string Filter4;
  public int? OutputValue;
}
```

- Reuse different Rule models with another set of strongly typed filters.

- Find best matching rule (respecting rule priority) and its
OutputValue for the set of input values.


### Prerequisites

- .NET 6.0 [[Installer](https://dotnet.microsoft.com/en-us/

### Verifying the installs

```shell
dotnet --info
``` 
prints out detailed information about a .NET installation and the machine environmen

### Build

Once you have all the required software installed, navigate to the project root and run:

```shell
dotnet build
```

This will install all the dependencies and build the solution.


### Artifacts

- `Rules` - a set of generic templates that can accept from 1 to 4 different filter types;
- `IStorage` - an interface for storing rules. There are several implementation of IStorage: `MemStorage`, `CacheStorage`, `SqlStorage`.
- `EngineStrategy` - an array of classes for processing rules. Provides LoadRules & FindRule methods. Can handle various types of filters.
- Various helper static classes (`DataReader`, `Generator`, `Replacer`, etc)

#### Registration & usage

Register strategy with predefine types of filters (TFilter1, TFilter2, etc). 
The maximum supported number of types is 4.

```cs
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddOptions<AppConfig>().Bind(config);
        services.AddStrategy(typeof(TFilter1), typeof(TFilter2), typeof(TFilter3), typeof(TFilter4));
    })
    .Build();

var strategy = services.GetRequiredService<IStrategy4<string, string, string, int>>();
try
{
    await strategy.LoadRules(csvFile);
    var rule = strategy.FindRule("BBB", "CCC", "CCC", 10);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
```

### Benchmarks

Benchmarking are available by default in Library.Benchmark.

To start benchmarks 

```shell
cd Library.Benchmark
dotnet run
```

