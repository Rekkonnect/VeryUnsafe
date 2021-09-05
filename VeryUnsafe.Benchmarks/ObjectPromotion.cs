using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace VeryUnsafe.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(exportHtml: true)]
    public class ObjectPromotion
    {
        private readonly RecordA baseInstance = new(1, 5);
        private readonly RecordA baseInstanceUnsafe = new(1, 5);

        [Benchmark(Baseline = true)]
        public RecordB PromoteViaCopying()
        {
            return baseInstance.PromoteToB();
        }
        [Benchmark]
        public RecordB PromoteUnsafe()
        {
            return VeryUnsafe.ChangeType<RecordB>(baseInstanceUnsafe);
        }

        public record RecordA(int A, int B)
        {
            public RecordB PromoteToB() => new(A, B);
        }
        public record RecordB(int A, int B) : RecordA(A, B);
    }
}
