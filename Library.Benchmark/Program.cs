using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Library.Benchmark;

var config = new ManualConfig()
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddColumnProvider(DefaultColumnProviders.Instance)
        .AddExporter(CsvExporter.Default)
        .AddLogger(ConsoleLogger.Default);

BenchmarkRunner.Run<FindRuleBenchmark>(config);
BenchmarkRunner.Run<LoadRulesBenchmark>(config);

