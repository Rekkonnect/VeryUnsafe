using BenchmarkDotNet.Running;

namespace Danger.VeryUnsafe.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ObjectAllocation>();
    }
}
