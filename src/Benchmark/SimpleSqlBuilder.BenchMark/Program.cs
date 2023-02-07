using BenchmarkDotNet.Running;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Will be removed when upgraded to .net 7.
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
