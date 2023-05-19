# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `Builder` and `FluentBuilder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).
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

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1702)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.302
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-GENUID : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-RUVSLE : .NET Framework 4.8.1 (4.8.9139.0), X64 RyuJIT VectorSize=256

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
|                SqlBuilder (Dapper) |             .NET 7.0 | Simple query |  1.870 μs | 0.0176 μs | 0.0138 μs |  1.00 |    0.00 |  0.6332 | 0.0038 |   2.91 KB |        1.00 |
|                            Builder |             .NET 7.0 | Simple query |  1.628 μs | 0.0316 μs | 0.0376 μs |  0.86 |    0.02 |  1.0834 | 0.0134 |   4.98 KB |        1.71 |
|                      FluentBuilder |             .NET 7.0 | Simple query |  2.004 μs | 0.0260 μs | 0.0243 μs |  1.07 |    0.01 |  0.9766 | 0.0114 |    4.5 KB |        1.54 |
|         Builder (Reuse parameters) |             .NET 7.0 | Simple query |  2.191 μs | 0.0098 μs | 0.0091 μs |  1.17 |    0.01 |  1.1406 | 0.0153 |   5.26 KB |        1.80 |
|   FluentBuilder (Reuse parameters) |             .NET 7.0 | Simple query |  2.614 μs | 0.0127 μs | 0.0112 μs |  1.40 |    0.01 |  1.0376 | 0.0153 |   4.77 KB |        1.64 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 | Simple query |  3.195 μs | 0.0070 μs | 0.0066 μs |  1.71 |    0.01 |  0.7439 | 0.0038 |   3.43 KB |        1.18 |
|                            Builder | .NET Framework 4.6.1 | Simple query |  4.327 μs | 0.0207 μs | 0.0193 μs |  2.32 |    0.02 |  1.1978 | 0.0076 |   5.55 KB |        1.90 |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |  4.420 μs | 0.0115 μs | 0.0102 μs |  2.36 |    0.02 |  1.1215 | 0.0076 |    5.2 KB |        1.79 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.083 μs | 0.0343 μs | 0.0304 μs |  2.72 |    0.03 |  1.3275 | 0.0153 |   6.12 KB |        2.10 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.199 μs | 0.0232 μs | 0.0217 μs |  2.78 |    0.02 |  1.2512 | 0.0153 |   5.77 KB |        1.98 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) |             .NET 7.0 |  Large query | 28.757 μs | 0.5676 μs | 0.5574 μs |  1.00 |    0.00 |  9.1553 | 0.8240 |  42.19 KB |        1.00 |
|                            Builder |             .NET 7.0 |  Large query | 22.569 μs | 0.0668 μs | 0.0558 μs |  0.78 |    0.02 | 14.1296 | 2.3193 |  65.04 KB |        1.54 |
|                      FluentBuilder |             .NET 7.0 |  Large query | 27.824 μs | 0.1149 μs | 0.1075 μs |  0.97 |    0.02 | 10.5591 | 1.3123 |  48.62 KB |        1.15 |
|         Builder (Reuse parameters) |             .NET 7.0 |  Large query | 15.511 μs | 0.2035 μs | 0.1804 μs |  0.54 |    0.01 |  9.8877 | 0.7324 |  45.59 KB |        1.08 |
|   FluentBuilder (Reuse parameters) |             .NET 7.0 |  Large query | 20.451 μs | 0.1011 μs | 0.0946 μs |  0.71 |    0.01 |  6.3477 | 0.2441 |  29.18 KB |        0.69 |
|                                    |                      |              |           |           |           |       |         |         |        |           |             |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 |  Large query | 44.623 μs | 0.1864 μs | 0.1653 μs |  1.55 |    0.03 | 11.4746 | 0.9766 |  53.09 KB |        1.26 |
|                            Builder | .NET Framework 4.6.1 |  Large query | 63.220 μs | 0.2288 μs | 0.2028 μs |  2.20 |    0.04 | 16.1133 | 2.1973 |  74.55 KB |        1.77 |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 65.000 μs | 0.2266 μs | 0.1769 μs |  2.25 |    0.05 | 14.7705 | 1.7090 |  68.61 KB |        1.63 |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 47.560 μs | 0.1043 μs | 0.0925 μs |  1.65 |    0.03 | 10.8032 | 0.7324 |  49.83 KB |        1.18 |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 50.538 μs | 0.1486 μs | 0.1390 μs |  1.76 |    0.03 |  9.4604 | 0.3662 |  43.87 KB |        1.04 |

