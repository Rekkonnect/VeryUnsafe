using BenchmarkDotNet.Running;

namespace VeryUnsafe.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ObjectPromotion>();
        }
    }
}
