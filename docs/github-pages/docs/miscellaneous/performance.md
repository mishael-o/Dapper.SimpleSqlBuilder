# Performance

Performance is always relative and depends on the scenario and other factors (e.g., hardware, OS, etc), however, the results below give a good indication of how the library performs.

The benchmark below shows the performance of the [Builder](../builders/builder.md) and [Fluent Builder](../builders/fluent-builder/fluent-builder.md) compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).

``` ini

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-FMZPXU : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-IXZORU : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256

```

|                             Method |              Runtime |   Categories |      Mean | Allocated |
|----------------------------------- |--------------------- |------------- |----------:|----------:|
|                SqlBuilder (Dapper) |             .NET 8.0 | Simple query |  1.672 μs |   2.92 KB |
|                            Builder |             .NET 8.0 | Simple query |  1.397 μs |   4.42 KB |
|                      FluentBuilder |             .NET 8.0 | Simple query |  1.688 μs |   4.49 KB |
|         Builder (Reuse parameters) |             .NET 8.0 | Simple query |  1.994 μs |    4.7 KB |
|   FluentBuilder (Reuse parameters) |             .NET 8.0 | Simple query |  2.325 μs |   4.77 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 | Simple query |  3.582 μs |   3.43 KB |
|                            Builder | .NET Framework 4.6.1 | Simple query |  4.212 μs |   4.69 KB |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |  4.787 μs |    5.2 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  4.854 μs |   5.27 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.808 μs |   5.77 KB |
|                                    |                      |              |           |           |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) |             .NET 8.0 |  Large query | 25.555 μs |  42.19 KB |
|                            Builder |             .NET 8.0 |  Large query | 18.822 μs |  48.78 KB |
|                      FluentBuilder |             .NET 8.0 |  Large query | 23.955 μs |  48.61 KB |
|         Builder (Reuse parameters) |             .NET 8.0 |  Large query | 13.726 μs |  29.34 KB |
|   FluentBuilder (Reuse parameters) |             .NET 8.0 |  Large query | 17.254 μs |  29.17 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 |  Large query | 45.213 μs |   53.1 KB |
|                            Builder | .NET Framework 4.6.1 |  Large query | 53.783 μs |  62.15 KB |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 65.146 μs |   68.6 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 40.245 μs |  37.42 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 52.934 μs |  43.86 KB |

Refer to the [benchmark project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Benchmark/SimpleSqlBuilder.BenchMark) for more information.
