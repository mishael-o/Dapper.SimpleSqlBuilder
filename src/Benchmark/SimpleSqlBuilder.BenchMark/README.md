# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `SimpleSqlBuilder` (`Builder` and `FluentBuilder`) compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark sql execution**).
The benchmark was done with the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The benchmark results may vary depending on the system configuration, OS, and other factors but the result below gives a general indication of performance.

To run the benchmark you will need to ensure you have the **corresponding SDKs for the frameworks** installed, then you can execute the command below in the benchmark project directory.

```cli
dotnet run -c release -f net6.0 -- --filter * --runtimes net461 net6.0
```

You can also run the benchmark only for a specific runtime by specifying the runtime in the `--runtimes` argument.

```cli
dotnet run -c release -f net6.0 -- --filter * --runtimes net6.0
```

## Result

``` ini

BenchmarkDotNet=v0.13.3, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  Job-QHREMU : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  Job-KJPETN : .NET Framework 4.8.1 (4.8.9105.0), X64 RyuJIT VectorSize=256


```

|                             Method |              Runtime |   Categories |       Mean |     Error |    StdDev | Ratio |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|----------------------------------- |--------------------- |------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
|              SqlBuilder (Dapper)   |             .NET 6.0 | Simple query |   1.878 μs | 0.0085 μs | 0.0076 μs |  0.58 |  0.6332 | 0.0038 |   2.91 KB |        0.85 |
|              SqlBuilder (Dapper)   | .NET Framework 4.6.1 | Simple query |   3.240 μs | 0.0093 μs | 0.0083 μs |  1.00 |  0.7439 | 0.0038 |   3.43 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|                            Builder |             .NET 6.0 | Simple query |   2.369 μs | 0.0062 μs | 0.0055 μs |  0.54 |  1.0948 | 0.0153 |   5.03 KB |        0.91 |
|                            Builder | .NET Framework 4.6.1 | Simple query |   4.355 μs | 0.0108 μs | 0.0101 μs |  1.00 |  1.1978 | 0.0153 |   5.54 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|                      FluentBuilder |             .NET 6.0 | Simple query |   2.019 μs | 0.0095 μs | 0.0084 μs |  0.45 |  0.9918 | 0.0114 |   4.57 KB |        0.88 |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |   4.475 μs | 0.0239 μs | 0.0223 μs |  1.00 |  1.1215 | 0.0076 |    5.2 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|       Builder (Reuse parameters)   |             .NET 6.0 | Simple query |   2.975 μs | 0.0169 μs | 0.0141 μs |  0.57 |  1.1635 | 0.0191 |   5.36 KB |        0.88 |
|       Builder (Reuse parameters)   | .NET Framework 4.6.1 | Simple query |   5.202 μs | 0.0107 μs | 0.0100 μs |  1.00 |  1.3275 | 0.0229 |   6.12 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
| FluentBuilder (Reuse parameters)   |             .NET 6.0 | Simple query |   2.730 μs | 0.0128 μs | 0.0120 μs |  0.52 |  1.0643 | 0.0153 |    4.9 KB |        0.85 |
| FluentBuilder (Reuse parameters)   | .NET Framework 4.6.1 | Simple query |   5.244 μs | 0.0161 μs | 0.0150 μs |  1.00 |  1.2512 | 0.0153 |   5.77 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|              SqlBuilder (Dapper)   |             .NET 6.0 |  Large query |  69.882 μs | 0.3047 μs | 0.2850 μs |  0.65 | 21.6064 | 4.2725 |  99.62 KB |        0.80 |
|              SqlBuilder (Dapper)   | .NET Framework 4.6.1 |  Large query | 108.259 μs | 0.4801 μs | 0.4491 μs |  1.00 | 26.9775 | 5.1270 | 124.58 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|                            Builder |             .NET 6.0 |  Large query |  81.597 μs | 0.3918 μs | 0.3665 μs |  0.52 | 32.4707 | 0.3662 | 149.63 KB |        0.86 |
|                            Builder | .NET Framework 4.6.1 |  Large query | 156.363 μs | 0.6679 μs | 0.6247 μs |  1.00 | 37.5977 | 0.2441 |    174 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|                      FluentBuilder |             .NET 6.0 |  Large query |  71.088 μs | 0.1827 μs | 0.1619 μs |  0.44 | 28.3203 | 3.1738 |  130.7 KB |        0.80 |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 161.203 μs | 0.4152 μs | 0.3681 μs |  1.00 | 35.4004 | 0.2441 | 163.54 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
|       Builder (Reuse parameters)   |             .NET 6.0 |  Large query |  64.919 μs | 0.6533 μs | 0.5791 μs |  0.56 | 21.9727 | 3.6621 | 101.19 KB |        0.92 |
|       Builder (Reuse parameters)   | .NET Framework 4.6.1 |  Large query | 116.054 μs | 0.2800 μs | 0.2619 μs |  1.00 | 23.8037 | 3.9063 | 110.21 KB |        1.00 |
|                                    |                      |              |            |           |           |       |         |        |           |             |
| FluentBuilder (Reuse parameters)   |             .NET 6.0 |  Large query |  52.414 μs | 0.4462 μs | 0.4174 μs |  0.42 | 14.6484 | 1.5869 |  67.44 KB |        0.68 |
| FluentBuilder (Reuse parameters)   | .NET Framework 4.6.1 |  Large query | 123.881 μs | 0.2860 μs | 0.2233 μs |  1.00 | 21.4844 | 2.1973 |  99.78 KB |        1.00 |
