# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `Builder` and `Fluent Builder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).
The benchmark was done with the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library. The benchmark results may vary depending on the system configuration, OS, and other factors but the result below gives a general indication of performance.

To run the benchmark you will need to ensure you have the **corresponding SDKs for the frameworks** installed, then you can execute the command below in the benchmark project directory.

```cli
dotnet run -c release -f net8.0 -- --filter * --runtimes net8.0 net461
```

You can also run the benchmark only for a specific runtime by specifying the runtime in the `--runtimes` argument.

```cli
dotnet run -c release -f net8.0 -- --filter * --runtimes net8.0
```

## Result

``` ini

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-FMZPXU : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-IXZORU : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256

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
|                SqlBuilder (Dapper) |             .NET 8.0 | Simple query |  1.672 μs | 0.0182 μs | 0.0161 μs |  1.00 |    0.00 |  0.6351 | 0.0019 |   2.92 KB |        1.00 |
|                            Builder |             .NET 8.0 | Simple query |  1.397 μs | 0.0265 μs | 0.0294 μs |  0.84 |    0.02 |  0.9613 | 0.0114 |   4.42 KB |        1.51 |
|                      FluentBuilder |             .NET 8.0 | Simple query |  1.688 μs | 0.0094 μs | 0.0087 μs |  1.01 |    0.01 |  0.9766 | 0.0114 |   4.49 KB |        1.54 |
|         Builder (Reuse parameters) |             .NET 8.0 | Simple query |  1.994 μs | 0.0178 μs | 0.0149 μs |  1.19 |    0.02 |  1.0185 | 0.0114 |    4.7 KB |        1.61 |
|   FluentBuilder (Reuse parameters) |             .NET 8.0 | Simple query |  2.325 μs | 0.0447 μs | 0.0397 μs |  1.39 |    0.03 |  1.0338 | 0.0153 |   4.77 KB |        1.63 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 | Simple query |  3.582 μs | 0.0652 μs | 0.0609 μs |  2.14 |    0.05 |  0.7439 | 0.0038 |   3.43 KB |        1.17 |
|                            Builder | .NET Framework 4.6.1 | Simple query |  4.212 μs | 0.0602 μs | 0.0563 μs |  2.52 |    0.05 |  1.0147 | 0.0076 |   4.69 KB |        1.61 |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |  4.787 μs | 0.0704 μs | 0.0658 μs |  2.87 |    0.05 |  1.1215 | 0.0076 |    5.2 KB |        1.78 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  4.854 μs | 0.0385 μs | 0.0321 μs |  2.90 |    0.03 |  1.1368 | 0.0076 |   5.27 KB |        1.80 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.808 μs | 0.0650 μs | 0.0608 μs |  3.47 |    0.04 |  1.2512 | 0.0153 |   5.77 KB |        1.97 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) |             .NET 8.0 |  Large query | 25.555 μs | 0.3134 μs | 0.2932 μs |  1.00 |    0.00 |  9.1553 | 0.9155 |  42.19 KB |        1.00 |
|                            Builder |             .NET 8.0 |  Large query | 18.822 μs | 0.1908 μs | 0.1692 μs |  0.74 |    0.01 | 10.5896 | 1.2512 |  48.78 KB |        1.16 |
|                      FluentBuilder |             .NET 8.0 |  Large query | 23.955 μs | 0.1477 μs | 0.1309 μs |  0.94 |    0.01 | 10.5591 | 1.3123 |  48.61 KB |        1.15 |
|         Builder (Reuse parameters) |             .NET 8.0 |  Large query | 13.726 μs | 0.0776 μs | 0.0726 μs |  0.54 |    0.01 |  6.3782 | 0.2441 |  29.34 KB |        0.70 |
|   FluentBuilder (Reuse parameters) |             .NET 8.0 |  Large query | 17.254 μs | 0.1649 μs | 0.1377 μs |  0.68 |    0.01 |  6.3477 | 0.2441 |  29.17 KB |        0.69 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 |  Large query | 45.213 μs | 0.2373 μs | 0.2220 μs |  1.77 |    0.02 | 11.4746 | 0.9155 |   53.1 KB |        1.26 |
|                            Builder | .NET Framework 4.6.1 |  Large query | 53.783 μs | 0.3271 μs | 0.3060 μs |  2.10 |    0.03 | 13.4277 | 1.5869 |  62.15 KB |        1.47 |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 65.146 μs | 0.3088 μs | 0.2889 μs |  2.55 |    0.03 | 14.7705 | 1.5869 |   68.6 KB |        1.63 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 40.245 μs | 0.4828 μs | 0.4280 μs |  1.57 |    0.03 |  8.0566 | 0.3052 |  37.42 KB |        0.89 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 52.934 μs | 0.2385 μs | 0.2231 μs |  2.07 |    0.03 |  9.4604 | 0.3662 |  43.86 KB |        1.04 |
