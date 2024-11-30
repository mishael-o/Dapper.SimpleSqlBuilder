# Performance

Performance is always relative, depending on the scenario and other factors such as hardware, operating system, etc. However, the results below give a good indication of how the library performs.

The benchmark below compares the performance of building SQL queries using the [Builder](../builders/builder.md), [Fluent Builder](../builders/fluent-builder/fluent-builder.md), and Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder).

> [!NOTE]
> This benchmark does not measure SQL execution times.

``` ini

BenchmarkDotNet v0.13.12, Windows 11 (10.0.26100.2454)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.100
  [Host]     : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2
  Job-AYCPZN : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2
  Job-FCIXJG : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256

```

|                             Method |              Runtime |   Categories |      Mean | Allocated |
|----------------------------------- |--------------------- |------------- |----------:|----------:|
|                SqlBuilder (Dapper) |             .NET 9.0 | Simple query |  1.641 μs |   2.92 KB |
|                            Builder |             .NET 9.0 | Simple query |  1.223 μs |   4.42 KB |
|                      FluentBuilder |             .NET 9.0 | Simple query |  1.435 μs |    4.5 KB |
|         Builder (Reuse parameters) |             .NET 9.0 | Simple query |  1.900 μs |    4.7 KB |
|   FluentBuilder (Reuse parameters) |             .NET 9.0 | Simple query |  2.108 μs |   4.77 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.2 | Simple query |  3.448 μs |   3.43 KB |
|                            Builder | .NET Framework 4.6.2 | Simple query |  4.031 μs |   4.69 KB |
|                      FluentBuilder | .NET Framework 4.6.2 | Simple query |  4.698 μs |    5.2 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.2 | Simple query |  4.985 μs |   5.27 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.2 | Simple query |  5.805 μs |   5.77 KB |
|                                    |                      |              |           |           |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) |             .NET 9.0 |  Large query | 27.723 μs |  42.19 KB |
|                            Builder |             .NET 9.0 |  Large query | 16.502 μs |  48.78 KB |
|                      FluentBuilder |             .NET 9.0 |  Large query | 19.042 μs |  48.62 KB |
|         Builder (Reuse parameters) |             .NET 9.0 |  Large query | 13.741 μs |  29.34 KB |
|   FluentBuilder (Reuse parameters) |             .NET 9.0 |  Large query | 15.446 μs |  29.18 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.2 |  Large query | 47.106 μs |   53.1 KB |
|                            Builder | .NET Framework 4.6.2 |  Large query | 56.699 μs |  62.15 KB |
|                      FluentBuilder | .NET Framework 4.6.2 |  Large query | 68.462 μs |  68.61 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.2 |  Large query | 43.441 μs |  37.42 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.2 |  Large query | 53.723 μs |  43.87 KB |

Refer to the [benchmark project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Benchmark/SimpleSqlBuilder.BenchMark) for more information.
