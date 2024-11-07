using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<ProcessorBenchmarks>();

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();