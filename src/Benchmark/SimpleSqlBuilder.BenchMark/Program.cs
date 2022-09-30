using BenchmarkDotNet.Running;
using SimpleSqlBuilder.BenchMark.Benchmarks;

#pragma warning disable CA1502 // Avoid excessive complexity
#pragma warning disable CA1812 // Avoid uninstantiated internal classes

BenchmarkRunner.Run<SimpleSqlBuilderBenchmark>();

#pragma warning restore CA1502 // Avoid excessive complexity
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
