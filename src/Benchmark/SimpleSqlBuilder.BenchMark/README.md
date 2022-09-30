# Benchmark

The benchmark project is mainly to help with development and code optimisations.

The benchmark below shows the performance of the `SimpleSqlBuilder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark sql execution**).
The benchmark was done with the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The benchmark results may vary depending on the system configuration, OS, and other factors but the result below gives a general indication of performance.

To run the benchmark execute the command below in the project directory.

```cli
dotnet run -c Release
```

## Result

```ini
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.918/21H2)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
```

| Method                                               |       Mean |     Error |    StdDev |    Gen0 |    Gen1 | Allocated |
| ---------------------------------------------------- | ---------: | --------: | --------: | ------: | ------: | --------: |
| 'SqlBuilder (Dapper) - Simple query'                 |   1.658 μs | 0.0237 μs | 0.0221 μs |  0.5646 |  0.0038 |    2.6 KB |
| 'SimpleSqlBuilder - Simple query'                    |   1.952 μs | 0.0387 μs | 0.0591 μs |  0.8850 |  0.0114 |   4.08 KB |
| 'SimpleSqlBuilder - Simple query (Reuse parameters)' |   2.537 μs | 0.0179 μs | 0.0149 μs |  1.0681 |  0.0153 |   4.92 KB |
| 'SqlBuilder (Dapper) - Large query'                  |  84.578 μs | 0.5627 μs | 0.5263 μs | 59.6924 | 11.1084 | 274.55 KB |
| 'SimpleSqlBuilder - Large query'                     | 149.545 μs | 0.6471 μs | 0.6053 μs | 61.0352 | 20.2637 | 281.65 KB |
| 'SimpleSqlBuilder - Large query (Reuse parameters)'  | 195.550 μs | 0.9972 μs | 0.9328 μs | 63.7207 | 21.2402 | 293.99 KB |
