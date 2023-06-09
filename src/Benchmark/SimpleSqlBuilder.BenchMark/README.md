# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `Builder` and `Fluent Builder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).
The benchmark was done with the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The benchmark results may vary depending on the system configuration, OS, and other factors but the result below gives a general indication of performance.

To run the benchmark you will need to ensure you have the **corresponding SDKs for the frameworks** installed, then you can execute the command below in the benchmark project directory.

```cli
dotnet run -c release -f net7.0 -- --filter * --runtimes net7.0 net461
```

You can also run the benchmark only for a specific runtime by specifying the runtime in the `--runtimes` argument.

```cli
dotnet run -c release -f net7.0 -- --filter * --runtimes net7.0
```

## Result

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1778)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.302
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-UDVULW : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-ZBHUIE : .NET Framework 4.8.1 (4.8.9139.0), X64 RyuJIT VectorSize=256

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
  1 μs        : 1 Microsecond (0.000001 sec)

```

|                             Method |              Runtime |   Categories |      Mean |     Error |    StdDev | Ratio | RatioSD |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|----------------------------------- |--------------------- |------------- |----------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
|                SqlBuilder (Dapper) |             .NET 7.0 | Simple query |  1.865 μs | 0.0135 μs | 0.0119 μs |  1.00 |    0.00 |  0.6351 | 0.0019 |   2.92 KB |        1.00 |
|                            Builder |             .NET 7.0 | Simple query |  1.531 μs | 0.0048 μs | 0.0042 μs |  0.82 |    0.01 |  0.9632 | 0.0114 |   4.43 KB |        1.52 |
|                      FluentBuilder |             .NET 7.0 | Simple query |  2.001 μs | 0.0116 μs | 0.0109 μs |  1.07 |    0.01 |  0.9766 | 0.0114 |    4.5 KB |        1.54 |
|         Builder (Reuse parameters) |             .NET 7.0 | Simple query |  2.195 μs | 0.0065 μs | 0.0057 μs |  1.18 |    0.01 |  1.0223 | 0.0114 |    4.7 KB |        1.61 |
|   FluentBuilder (Reuse parameters) |             .NET 7.0 | Simple query |  2.755 μs | 0.0115 μs | 0.0108 μs |  1.48 |    0.01 |  1.0376 | 0.0153 |   4.77 KB |        1.63 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 | Simple query |  3.237 μs | 0.0148 μs | 0.0138 μs |  1.74 |    0.01 |  0.7439 | 0.0038 |   3.43 KB |        1.17 |
|                            Builder | .NET Framework 4.6.1 | Simple query |  3.821 μs | 0.0193 μs | 0.0181 μs |  2.05 |    0.02 |  1.0147 | 0.0076 |    4.7 KB |        1.61 |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |  4.493 μs | 0.0109 μs | 0.0096 μs |  2.41 |    0.01 |  1.1215 | 0.0076 |    5.2 KB |        1.78 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  4.607 μs | 0.0122 μs | 0.0114 μs |  2.47 |    0.01 |  1.1368 | 0.0153 |   5.27 KB |        1.80 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.260 μs | 0.0149 μs | 0.0139 μs |  2.82 |    0.02 |  1.2512 | 0.0153 |   5.77 KB |        1.98 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) |             .NET 7.0 |  Large query | 28.193 μs | 0.1919 μs | 0.1701 μs |  1.00 |    0.00 |  9.1553 | 0.8240 |  42.19 KB |        1.00 |
|                            Builder |             .NET 7.0 |  Large query | 21.475 μs | 0.0956 μs | 0.0848 μs |  0.76 |    0.01 | 10.5896 | 1.1902 |  48.79 KB |        1.16 |
|                      FluentBuilder |             .NET 7.0 |  Large query | 26.700 μs | 0.0820 μs | 0.0684 μs |  0.95 |    0.01 | 10.5591 | 1.3123 |  48.62 KB |        1.15 |
|         Builder (Reuse parameters) |             .NET 7.0 |  Large query | 14.929 μs | 0.0796 μs | 0.0705 μs |  0.53 |    0.00 |  6.3782 | 0.2441 |  29.34 KB |        0.70 |
|   FluentBuilder (Reuse parameters) |             .NET 7.0 |  Large query | 20.039 μs | 0.0544 μs | 0.0509 μs |  0.71 |    0.00 |  6.3477 | 0.2441 |  29.18 KB |        0.69 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 |  Large query | 43.275 μs | 0.0969 μs | 0.0809 μs |  1.53 |    0.01 | 11.4746 | 0.9155 |   53.1 KB |        1.26 |
|                            Builder | .NET Framework 4.6.1 |  Large query | 52.571 μs | 0.1201 μs | 0.1123 μs |  1.86 |    0.01 | 13.4277 | 1.6479 |  62.15 KB |        1.47 |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 63.775 μs | 0.2096 μs | 0.1961 μs |  2.26 |    0.01 | 14.7705 | 1.7090 |  68.61 KB |        1.63 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 39.589 μs | 0.0854 μs | 0.0799 μs |  1.40 |    0.01 |  8.0566 | 0.3052 |  37.42 KB |        0.89 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 50.712 μs | 0.1661 μs | 0.1553 μs |  1.80 |    0.01 |  9.4604 | 0.3662 |  43.87 KB |        1.04 |
