# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `Builder` and `FluentBuilder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).
The benchmark was done with the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The benchmark results may vary depending on the system configuration, OS, and other factors but the result below gives a general indication of performance.

To run the benchmark you will need to ensure you have the **corresponding SDKs for the frameworks** installed, then you can execute the command below in the benchmark project directory.

```cli
dotnet run -c release -f net6.0 -- --filter * --runtimes net6.0 net461
```

You can also run the benchmark only for a specific runtime by specifying the runtime in the `--runtimes` argument.

```cli
dotnet run -c release -f net6.0 -- --filter * --runtimes net6.0
```

## Result

``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.308
  [Host]     : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  Job-GEPLMO : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  Job-FSXWVC : .NET Framework 4.8.1 (4.8.9105.0), X64 RyuJIT VectorSize=256

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
|                SqlBuilder (Dapper) |             .NET 6.0 | Simple query |  1.922 μs | 0.0167 μs | 0.0148 μs |  1.00 |    0.00 |  0.6332 | 0.0038 |   2.91 KB |        1.00 |
|                            Builder |             .NET 6.0 | Simple query |  1.647 μs | 0.0054 μs | 0.0045 μs |  0.86 |    0.01 |  1.0986 | 0.0172 |   5.05 KB |        1.73 |
|                      FluentBuilder |             .NET 6.0 | Simple query |  2.017 μs | 0.0149 μs | 0.0124 μs |  1.05 |    0.01 |  0.9918 | 0.0114 |   4.57 KB |        1.57 |
|         Builder (Reuse parameters) |             .NET 6.0 | Simple query |  2.356 μs | 0.0107 μs | 0.0095 μs |  1.23 |    0.01 |  1.1711 | 0.0191 |   5.38 KB |        1.85 |
|   FluentBuilder (Reuse parameters) |             .NET 6.0 | Simple query |  2.727 μs | 0.0213 μs | 0.0199 μs |  1.42 |    0.01 |  1.0643 | 0.0153 |    4.9 KB |        1.68 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 | Simple query |  3.310 μs | 0.0640 μs | 0.0567 μs |  1.72 |    0.03 |  0.7439 | 0.0038 |   3.43 KB |        1.18 |
|                            Builder | .NET Framework 4.6.1 | Simple query |  4.373 μs | 0.0112 μs | 0.0093 μs |  2.28 |    0.02 |  1.1978 | 0.0153 |   5.55 KB |        1.90 |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |  4.475 μs | 0.0183 μs | 0.0153 μs |  2.33 |    0.02 |  1.1215 | 0.0076 |    5.2 KB |        1.79 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.205 μs | 0.0171 μs | 0.0143 μs |  2.71 |    0.02 |  1.3275 | 0.0229 |   6.12 KB |        2.10 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.288 μs | 0.0385 μs | 0.0321 μs |  2.75 |    0.02 |  1.2512 | 0.0153 |   5.77 KB |        1.98 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) |             .NET 6.0 |  Large query | 28.648 μs | 0.1276 μs | 0.1066 μs |  1.00 |    0.00 |  9.1553 | 0.7019 |  42.19 KB |        1.00 |
|                            Builder |             .NET 6.0 |  Large query | 23.737 μs | 0.1167 μs | 0.0975 μs |  0.83 |    0.00 | 14.3738 | 2.3804 |  66.15 KB |        1.57 |
|                      FluentBuilder |             .NET 6.0 |  Large query | 28.695 μs | 0.1140 μs | 0.0952 μs |  1.00 |    0.01 | 10.8032 | 1.5259 |  49.73 KB |        1.18 |
|         Builder (Reuse parameters) |             .NET 6.0 |  Large query | 17.750 μs | 0.1673 μs | 0.1565 μs |  0.62 |    0.01 | 10.1624 | 1.0071 |  46.76 KB |        1.11 |
|   FluentBuilder (Reuse parameters) |             .NET 6.0 |  Large query | 22.434 μs | 0.1823 μs | 0.1522 μs |  0.78 |    0.01 |  6.5918 | 0.3662 |  30.34 KB |        0.72 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 |  Large query | 44.050 μs | 0.3966 μs | 0.3312 μs |  1.54 |    0.01 | 11.4746 | 0.9766 |  53.09 KB |        1.26 |
|                            Builder | .NET Framework 4.6.1 |  Large query | 62.126 μs | 0.2266 μs | 0.1770 μs |  2.17 |    0.01 | 16.1133 | 0.4883 |  74.55 KB |        1.77 |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 65.133 μs | 0.2885 μs | 0.2698 μs |  2.27 |    0.02 | 14.7705 | 1.9531 |  68.61 KB |        1.63 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 48.110 μs | 0.1603 μs | 0.1499 μs |  1.68 |    0.01 | 10.8032 | 1.0376 |  49.83 KB |        1.18 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 50.751 μs | 0.3210 μs | 0.2680 μs |  1.77 |    0.01 |  9.4604 | 0.5493 |  43.87 KB |        1.04 |
