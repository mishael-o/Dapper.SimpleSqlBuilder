# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `Builder` and `Fluent Builder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).
The benchmark was done with the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library. The results may vary depending on the system configuration, OS, and other factors, so they are indicative rather than absolute.

To run the benchmark you will need to ensure you have the **corresponding SDKs for the frameworks** installed, then you can execute the command below in the benchmark project directory.

```cli
dotnet run -c release -f net10.0 --runtimes net10.0 net48 -- --filter '*'
```

You can also run the benchmark only for a specific framework.

```cli
dotnet run -c release -f net10.0 --filter '*'
```

## Result

Each runtime was benchmarked separately, so `Ratio` and `Alloc Ratio` compare against Dapper's `SqlBuilder` on the **same runtime**.

``` ini

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8894/25H2/2025Update/HudsonValley2)
Intel Core Ultra 9 275HX 2.70GHz, 1 CPU, 24 logical and 24 physical cores
.NET SDK 10.0.302
  [Host]             : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  .NET 10.0          : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256

Legends:
  Categories  : All categories of the corresponded method, class, and assembly
  Mean        : Arithmetic mean of all measurements
  Error       : Half of 99.9% confidence interval
  StdDev      : Standard deviation of all measurements
  Ratio       : Mean of the ratio distribution ([Current]/[Baseline])
  RatioSD     : Standard deviation of the ratio distribution ([Current]/[Baseline])
  Gen0        : GC Generation 0 collects per 1000 operations
  Gen1        : GC Generation 1 collects per 1000 operations
  Allocated   : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  Alloc Ratio : Allocated memory ratio distribution ([Current]/[Baseline])
  1 ns        : 1 Nanosecond (0.000000001 sec)

```

| Method                           | Runtime            | Categories   | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|--------------------------------- |------------------- |------------- |------------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| SqlBuilder (Dapper)              | .NET 10.0          | Simple query |    332.7 ns |   5.17 ns |   4.83 ns |  1.00 |    0.02 |  0.1569 | 0.0010 |   2.89 KB |        1.00 |
| Builder                          | .NET 10.0          | Simple query |    417.6 ns |   6.18 ns |   5.48 ns |  1.26 |    0.02 |  0.1903 | 0.0014 |   3.55 KB |        1.23 |
| FluentBuilder                    | .NET 10.0          | Simple query |    507.2 ns |   6.97 ns |   6.52 ns |  1.52 |    0.03 |  0.2308 | 0.0029 |    4.3 KB |        1.49 |
| Builder (Reuse parameters)       | .NET 10.0          | Simple query |    713.1 ns |   9.51 ns |   8.89 ns |  2.14 |    0.04 |  0.2394 | 0.0029 |   4.45 KB |        1.54 |
| FluentBuilder (Reuse parameters) | .NET 10.0          | Simple query |    769.7 ns |   6.05 ns |   5.05 ns |  2.31 |    0.04 |  0.2441 | 0.0029 |   4.54 KB |        1.57 |
|                                  |                    |              |             |           |           |       |         |         |        |           |             |
| SqlBuilder (Dapper)              | .NET Framework 4.8 | Simple query |    878.4 ns |   3.41 ns |   3.03 ns |  1.00 |    0.00 |  0.5054 | 0.0048 |   3.11 KB |        1.00 |
| Builder                          | .NET Framework 4.8 | Simple query |  1,647.1 ns |   4.14 ns |   3.46 ns |  1.88 |    0.01 |  0.7420 | 0.0076 |   4.56 KB |        1.47 |
| FluentBuilder                    | .NET Framework 4.8 | Simple query |  2,038.0 ns |  10.70 ns |   9.49 ns |  2.32 |    0.01 |  0.8430 | 0.0076 |    5.2 KB |        1.67 |
| Builder (Reuse parameters)       | .NET Framework 4.8 | Simple query |  2,266.5 ns |   5.35 ns |   4.47 ns |  2.58 |    0.01 |  0.8392 | 0.0114 |   5.18 KB |        1.66 |
| FluentBuilder (Reuse parameters) | .NET Framework 4.8 | Simple query |  2,538.5 ns |  13.82 ns |  12.25 ns |  2.89 |    0.02 |  0.8278 | 0.0076 |   5.11 KB |        1.64 |
|                                  |                    |              |             |           |           |       |         |         |        |           |             |
|                                  |                    |              |             |           |           |       |         |         |        |           |             |
| SqlBuilder (Dapper)              | .NET 10.0          | Large query  |  4,057.1 ns |  80.33 ns |  85.95 ns |  1.00 |    0.03 |  2.3270 | 0.1907 |  42.38 KB |        1.00 |
| Builder                          | .NET 10.0          | Large query  |  6,220.9 ns | 111.21 ns | 104.03 ns |  1.53 |    0.04 |  2.4414 | 0.2747 |  44.45 KB |        1.05 |
| FluentBuilder                    | .NET 10.0          | Large query  |  7,117.7 ns |  82.71 ns |  69.06 ns |  1.76 |    0.04 |  2.4414 | 0.2899 |   44.3 KB |        1.05 |
| Builder (Reuse parameters)       | .NET 10.0          | Large query  |  5,314.7 ns |  73.72 ns |  68.96 ns |  1.31 |    0.03 |  1.0681 | 0.0381 |  19.74 KB |        0.47 |
| FluentBuilder (Reuse parameters) | .NET 10.0          | Large query  |  5,793.7 ns |  91.96 ns |  86.02 ns |  1.43 |    0.04 |  1.0605 | 0.0381 |  19.59 KB |        0.46 |
|                                  |                    |              |             |           |           |       |         |         |        |           |             |
| SqlBuilder (Dapper)              | .NET Framework 4.8 | Large query  |  9,870.6 ns |  69.25 ns |  61.38 ns |  1.00 |    0.01 |  7.5989 | 0.7477 |  46.75 KB |        1.00 |
| Builder                          | .NET Framework 4.8 | Large query  | 24,934.5 ns | 293.54 ns | 245.12 ns |  2.53 |    0.03 | 10.0708 | 1.1902 |  61.96 KB |        1.33 |
| FluentBuilder                    | .NET Framework 4.8 | Large query  | 29,487.7 ns | 147.57 ns | 130.82 ns |  2.99 |    0.02 | 11.1084 | 1.2207 |  68.39 KB |        1.46 |
| Builder (Reuse parameters)       | .NET Framework 4.8 | Large query  | 16,275.9 ns |  60.36 ns |  53.51 ns |  1.65 |    0.01 |  3.6926 | 0.1221 |  22.87 KB |        0.49 |
| FluentBuilder (Reuse parameters) | .NET Framework 4.8 | Large query  | 21,220.8 ns |  82.47 ns |  68.87 ns |  2.15 |    0.01 |  4.7607 | 0.1831 |  29.33 KB |        0.63 |
