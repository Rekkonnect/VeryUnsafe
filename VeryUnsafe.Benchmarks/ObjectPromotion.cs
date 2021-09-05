using BenchmarkDotNet.Attributes;

namespace VeryUnsafe.Benchmarks
{
    [MemoryDiagnoser]
    public class ObjectPromotion
    {
        private RecordA baseInstance = new(1, 5);
        private RecordA baseInstanceUnsafe = new(1, 5);

        [Benchmark(Baseline = true)]
        public RecordB PromoteViaCopying()
        {
            return baseInstance.PromoteToB();
        }
        [Benchmark]
        public RecordB PromoteUnsafe()
        {
            VeryUnsafe.ChangeType<RecordB>(baseInstanceUnsafe);
            return baseInstanceUnsafe as RecordB;
        }

        public record RecordA(int A, int B)
        {
            public RecordB PromoteToB() => new(A, B);
        }
        public record RecordB(int A, int B) : RecordA(A, B);
    }
}
