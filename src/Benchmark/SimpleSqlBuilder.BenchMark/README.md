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

| Method                             | Runtime            | Categories   | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|----------------------------------- |------------------- |------------- |----------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| SqlBuilder (Dapper)                | .NET 9.0           | Simple query |  1.614 us | 0.0274 us | 0.0229 us |  1.00 |    0.00 |  0.6371 | 0.0038 |   2.93 KB |        1.00 |
| Builder                            | .NET 9.0           | Simple query |  1.222 us | 0.0164 us | 0.0145 us |  0.76 |    0.02 |  0.9785 | 0.0114 |    4.5 KB |        1.54 |
| FluentBuilder                      | .NET 9.0           | Simple query |  1.390 us | 0.0185 us | 0.0164 us |  0.86 |    0.02 |  0.9975 | 0.0134 |   4.59 KB |        1.57 |
| Builder (Reuse parameters)         | .NET 9.0           | Simple query |  1.842 us | 0.0193 us | 0.0171 us |  1.14 |    0.01 |  1.0376 | 0.0153 |   4.77 KB |        1.63 |
| FluentBuilder (Reuse parameters)   | .NET 9.0           | Simple query |  1.987 us | 0.0386 us | 0.0361 us |  1.23 |    0.03 |  1.0567 | 0.0153 |   4.86 KB |        1.66 |
|                                    |                    |              |           |           |           |       |         |         |        |           |             |
| SqlBuilder (Dapper)                | .NET Framework 4.8 | Simple query |  3.485 us | 0.0474 us | 0.0879 us |  2.19 |    0.08 |  0.7439 | 0.0038 |   3.44 KB |        1.17 |
| Builder                            | .NET Framework 4.8 | Simple query |  4.378 us | 0.0515 us | 0.0456 us |  2.71 |    0.05 |  1.1520 | 0.0076 |   5.32 KB |        1.82 |
| FluentBuilder                      | .NET Framework 4.8 | Simple query |  4.830 us | 0.0536 us | 0.0502 us |  2.99 |    0.05 |  1.1368 | 0.0076 |   5.25 KB |        1.79 |
| Builder (Reuse parameters)         | .NET Framework 4.8 | Simple query |  5.134 us | 0.0772 us | 0.0685 us |  3.18 |    0.06 |  1.2741 | 0.0153 |   5.89 KB |        2.01 |
| FluentBuilder (Reuse parameters)   | .NET Framework 4.8 | Simple query |  5.799 us | 0.0291 us | 0.0243 us |  3.59 |    0.05 |  1.2589 | 0.0153 |   5.82 KB |        1.99 |
|                                    |                    |              |           |           |           |       |         |         |        |           |             |
|                                    |                    |              |           |           |           |       |         |         |        |           |             |
| SqlBuilder (Dapper)                | .NET 9.0           | Large query  | 27.432 μs | 0.1997 μs | 0.1868 μs |  1.00 |    0.00 |  9.2163 | 0.7629 |  42.42 KB |        1.00 |
| Builder                            | .NET 9.0           | Large query  | 16.493 μs | 0.2553 μs | 0.2264 μs |  0.60 |    0.01 | 10.6506 | 1.1597 |  49.05 KB |        1.16 |
| FluentBuilder                      | .NET 9.0           | Large query  | 18.964 μs | 0.2916 μs | 0.2728 μs |  0.69 |    0.01 | 10.6201 | 1.3123 |  48.89 KB |        1.15 |
| Builder (Reuse parameters)         | .NET 9.0           | Large query  | 12.842 μs | 0.1155 μs | 0.0902 μs |  0.47 |    0.01 |  6.3934 | 0.2594 |  29.41 KB |        0.69 |
| FluentBuilder (Reuse parameters)   | .NET 9.0           | Large query  | 14.713 μs | 0.1177 μs | 0.1044 μs |  0.54 |    0.01 |  6.3629 | 0.2441 |   29.3 KB |        0.69 |
|                                    |                    |              |           |           |           |       |         |         |        |           |             |
| SqlBuilder (Dapper)                | .NET Framework 4.8 | Large query  | 46.692 μs | 0.3956 μs | 0.3507 μs |  1.70 |    0.02 | 11.5356 | 1.0986 |  53.32 KB |        1.26 |
| Builder                            | .NET Framework 4.8 | Large query  | 58.544 μs | 0.3523 μs | 0.3123 μs |  2.14 |    0.02 | 13.4277 | 0.1221 |  61.96 KB |        1.46 |
| FluentBuilder                      | .NET Framework 4.8 | Large query  | 68.833 μs | 0.7452 μs | 0.6222 μs |  2.51 |    0.03 | 14.7705 | 1.7090 |  68.43 KB |        1.61 |
| Builder (Reuse parameters)         | .NET Framework 4.8 | Large query  | 44.878 μs | 0.4036 μs | 0.3578 μs |  1.64 |    0.02 |  7.9956 | 0.3052 |  37.13 KB |        0.88 |
| FluentBuilder (Reuse parameters)   | .NET Framework 4.8 | Large query  | 55.460 μs | 0.4013 μs | 0.3753 μs |  2.02 |    0.02 |  9.4604 | 0.3662 |  43.63 KB |        1.03 |