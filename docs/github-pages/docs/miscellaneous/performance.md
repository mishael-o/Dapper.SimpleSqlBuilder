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

Legends:
  Categories  : All categories of the corresponded method, class, and assembly
  Mean        : Arithmetic mean of all measurements
  Allocated   : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 μs        : 1 Microsecond (0.000001 sec)

```

| Method                             | Runtime            | Categories   | Mean      | Allocated |
|----------------------------------- |------------------- |------------- |----------:|----------:|
| SqlBuilder (Dapper)                | .NET 9.0           | Simple query |  1.614 us |   2.93 KB |
| Builder                            | .NET 9.0           | Simple query |  1.222 us |    4.5 KB |
| FluentBuilder                      | .NET 9.0           | Simple query |  1.390 us |   4.59 KB |
| Builder (Reuse parameters)         | .NET 9.0           | Simple query |  1.842 us |   4.77 KB |
| FluentBuilder (Reuse parameters)   | .NET 9.0           | Simple query |  1.987 us |   4.86 KB |
|                                    |                    |              |           |           |
| SqlBuilder (Dapper)                | .NET Framework 4.8 | Simple query |  3.485 us |   3.44 KB |
| Builder                            | .NET Framework 4.8 | Simple query |  4.378 us |   5.32 KB |
| FluentBuilder                      | .NET Framework 4.8 | Simple query |  4.830 us |   5.25 KB |
| Builder (Reuse parameters)         | .NET Framework 4.8 | Simple query |  5.134 us |   5.89 KB |
| FluentBuilder (Reuse parameters)   | .NET Framework 4.8 | Simple query |  5.799 us |   5.82 KB |
|                                    |                    |              |           |           |
|                                    |                    |              |           |           |
| SqlBuilder (Dapper)                | .NET 9.0           | Large query  | 27.432 μs |  42.42 KB |
| Builder                            | .NET 9.0           | Large query  | 16.493 μs |  49.05 KB |
| FluentBuilder                      | .NET 9.0           | Large query  | 18.964 μs |  48.89 KB |
| Builder (Reuse parameters)         | .NET 9.0           | Large query  | 12.842 μs |  29.41 KB |
| FluentBuilder (Reuse parameters)   | .NET 9.0           | Large query  | 14.713 μs |   29.3 KB |
|                                    |                    |              |           |           |
| SqlBuilder (Dapper)                | .NET Framework 4.8 | Large query  | 46.692 μs |  53.32 KB |
| Builder                            | .NET Framework 4.8 | Large query  | 58.544 μs |  61.96 KB |
| FluentBuilder                      | .NET Framework 4.8 | Large query  | 68.833 μs |  68.43 KB |
| Builder (Reuse parameters)         | .NET Framework 4.8 | Large query  | 44.878 μs |  37.13 KB |
| FluentBuilder (Reuse parameters)   | .NET Framework 4.8 | Large query  | 55.460 μs |  43.63 KB |

Refer to the [benchmark project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Benchmark/SimpleSqlBuilder.BenchMark) for more information.
