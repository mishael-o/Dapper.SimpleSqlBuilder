# Performance

Performance is always relative, depending on the scenario and other factors such as hardware, operating system, etc. The results below are therefore indicative rather than absolute.

The benchmark below compares the performance of building SQL queries using the [Builder](../builders/builder.md), [Fluent Builder](../builders/fluent-builder/fluent-builder.md), and Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder).

> [!NOTE]
> This benchmark does not measure SQL execution times.

Each runtime was benchmarked separately, so `Ratio` compares against Dapper's `SqlBuilder` on the same runtime.

``` ini

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8894/25H2/2025Update/HudsonValley2)
Intel Core Ultra 9 275HX 2.70GHz, 1 CPU, 24 logical and 24 physical cores
.NET SDK 10.0.302
  [Host]             : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  .NET 10.0          : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256

Legends:
  Categories  : All categories of the corresponded method, class, and assembly
  Mean        : Arithmetic mean of all measurements
  Ratio       : Mean of the ratio distribution ([Current]/[Baseline])
  Allocated   : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ns        : 1 Nanosecond (0.000000001 sec)

```

| Method                           | Runtime            | Categories   | Mean        | Ratio | Allocated |
|--------------------------------- |------------------- |------------- |------------:|------:|----------:|
| SqlBuilder (Dapper)              | .NET 10.0          | Simple query |    332.7 ns |  1.00 |   2.89 KB |
| Builder                          | .NET 10.0          | Simple query |    417.6 ns |  1.26 |   3.55 KB |
| FluentBuilder                    | .NET 10.0          | Simple query |    507.2 ns |  1.52 |    4.3 KB |
| Builder (Reuse parameters)       | .NET 10.0          | Simple query |    713.1 ns |  2.14 |   4.45 KB |
| FluentBuilder (Reuse parameters) | .NET 10.0          | Simple query |    769.7 ns |  2.31 |   4.54 KB |
|                                  |                    |              |             |       |           |
| SqlBuilder (Dapper)              | .NET Framework 4.8 | Simple query |    878.4 ns |  1.00 |   3.11 KB |
| Builder                          | .NET Framework 4.8 | Simple query |  1,647.1 ns |  1.88 |   4.56 KB |
| FluentBuilder                    | .NET Framework 4.8 | Simple query |  2,038.0 ns |  2.32 |    5.2 KB |
| Builder (Reuse parameters)       | .NET Framework 4.8 | Simple query |  2,266.5 ns |  2.58 |   5.18 KB |
| FluentBuilder (Reuse parameters) | .NET Framework 4.8 | Simple query |  2,538.5 ns |  2.89 |   5.11 KB |
|                                  |                    |              |             |       |           |
|                                  |                    |              |             |       |           |
| SqlBuilder (Dapper)              | .NET 10.0          | Large query  |  4,057.1 ns |  1.00 |  42.38 KB |
| Builder                          | .NET 10.0          | Large query  |  6,220.9 ns |  1.53 |  44.45 KB |
| FluentBuilder                    | .NET 10.0          | Large query  |  7,117.7 ns |  1.76 |   44.3 KB |
| Builder (Reuse parameters)       | .NET 10.0          | Large query  |  5,314.7 ns |  1.31 |  19.74 KB |
| FluentBuilder (Reuse parameters) | .NET 10.0          | Large query  |  5,793.7 ns |  1.43 |  19.59 KB |
|                                  |                    |              |             |       |           |
| SqlBuilder (Dapper)              | .NET Framework 4.8 | Large query  |  9,870.6 ns |  1.00 |  46.75 KB |
| Builder                          | .NET Framework 4.8 | Large query  | 24,934.5 ns |  2.53 |  61.96 KB |
| FluentBuilder                    | .NET Framework 4.8 | Large query  | 29,487.7 ns |  2.99 |  68.39 KB |
| Builder (Reuse parameters)       | .NET Framework 4.8 | Large query  | 16,275.9 ns |  1.65 |  22.87 KB |
| FluentBuilder (Reuse parameters) | .NET Framework 4.8 | Large query  | 21,220.8 ns |  2.15 |  29.33 KB |

Refer to the [benchmark project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Benchmark/SimpleSqlBuilder.BenchMark) for more information.
