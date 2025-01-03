# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `Builder` and `Fluent Builder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).
The benchmark was done with the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library. The benchmark results may vary depending on the system configuration, OS, and other factors but the result below gives a general indication of performance.

To run the benchmark you will need to ensure you have the **corresponding SDKs for the frameworks** installed, then you can execute the command below in the benchmark project directory.

```cli
dotnet run -c release -f net9.0 --runtimes net9.0 net48 -- --filter '*'
```

You can also run the benchmark only for a specific framework.

```cli
dotnet run -c release -f net9.0 --filter '*'
```

## Result

``` ini

BenchmarkDotNet v0.13.12, Windows 11 (10.0.26100.2454)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.100
  [Host]     : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2
  Job-AYCPZN : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2
  Job-FCIXJG : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256

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
|                SqlBuilder (Dapper) |             .NET 9.0 | Simple query |  1.641 μs | 0.0318 μs | 0.0298 μs |  1.00 |    0.00 |  0.6351 | 0.0019 |   2.92 KB |        1.00 |
|                            Builder |             .NET 9.0 | Simple query |  1.223 μs | 0.0240 μs | 0.0312 μs |  0.75 |    0.02 |  0.9613 | 0.0114 |   4.42 KB |        1.51 |
|                      FluentBuilder |             .NET 9.0 | Simple query |  1.435 μs | 0.0253 μs | 0.0224 μs |  0.87 |    0.02 |  0.9785 | 0.0114 |    4.5 KB |        1.54 |
|         Builder (Reuse parameters) |             .NET 9.0 | Simple query |  1.900 μs | 0.0359 μs | 0.0369 μs |  1.16 |    0.03 |  1.0204 | 0.0134 |    4.7 KB |        1.61 |
|   FluentBuilder (Reuse parameters) |             .NET 9.0 | Simple query |  2.108 μs | 0.0409 μs | 0.0546 μs |  1.28 |    0.05 |  1.0376 | 0.0153 |   4.77 KB |        1.63 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.2 | Simple query |  3.448 μs | 0.0606 μs | 0.0567 μs |  2.10 |    0.04 |  0.7439 | 0.0038 |   3.43 KB |        1.17 |
|                            Builder | .NET Framework 4.6.2 | Simple query |  4.031 μs | 0.0777 μs | 0.0832 μs |  2.46 |    0.06 |  1.0147 | 0.0076 |   4.69 KB |        1.61 |
|                      FluentBuilder | .NET Framework 4.6.2 | Simple query |  4.698 μs | 0.0932 μs | 0.0915 μs |  2.86 |    0.08 |  1.1215 | 0.0076 |    5.2 KB |        1.78 |
|         Builder (Reuse parameters) | .NET Framework 4.6.2 | Simple query |  4.985 μs | 0.0812 μs | 0.0760 μs |  3.04 |    0.08 |  1.1368 | 0.0076 |   5.27 KB |        1.80 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.2 | Simple query |  5.805 μs | 0.1126 μs | 0.1156 μs |  3.54 |    0.09 |  1.2512 | 0.0153 |   5.77 KB |        1.98 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) |             .NET 9.0 |  Large query | 27.723 μs | 0.3809 μs | 0.3563 μs |  1.00 |    0.00 |  9.1553 | 0.9155 |  42.19 KB |        1.00 |
|                            Builder |             .NET 9.0 |  Large query | 16.502 μs | 0.3285 μs | 0.5016 μs |  0.60 |    0.02 | 10.5896 | 1.2512 |  48.78 KB |        1.16 |
|                      FluentBuilder |             .NET 9.0 |  Large query | 19.042 μs | 0.3493 μs | 0.3268 μs |  0.69 |    0.02 | 10.5591 | 1.3123 |  48.62 KB |        1.15 |
|         Builder (Reuse parameters) |             .NET 9.0 |  Large query | 13.741 μs | 0.2241 μs | 0.1871 μs |  0.50 |    0.01 |  6.3782 | 0.2441 |  29.34 KB |        0.70 |
|   FluentBuilder (Reuse parameters) |             .NET 9.0 |  Large query | 15.446 μs | 0.2821 μs | 0.2501 μs |  0.56 |    0.01 |  6.3477 | 0.2441 |  29.18 KB |        0.69 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.2 |  Large query | 47.106 μs | 0.9295 μs | 0.8695 μs |  1.70 |    0.04 | 11.4746 | 0.9155 |   53.1 KB |        1.26 |
|                            Builder | .NET Framework 4.6.2 |  Large query | 56.699 μs | 0.8183 μs | 0.7254 μs |  2.05 |    0.03 | 13.4277 | 1.6479 |  62.15 KB |        1.47 |
|                      FluentBuilder | .NET Framework 4.6.2 |  Large query | 68.462 μs | 1.3275 μs | 1.8172 μs |  2.47 |    0.08 | 14.7705 | 1.7090 |  68.61 KB |        1.63 |
|         Builder (Reuse parameters) | .NET Framework 4.6.2 |  Large query | 43.441 μs | 0.8512 μs | 0.9802 μs |  1.57 |    0.05 |  8.0566 | 0.3052 |  37.42 KB |        0.89 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.2 |  Large query | 53.723 μs | 0.8383 μs | 0.7841 μs |  1.94 |    0.05 |  9.4604 | 0.3662 |  43.87 KB |        1.04 |
