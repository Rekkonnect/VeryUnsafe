using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace VeryUnsafe.Benchmarks
{
    [MemoryDiagnoser, DisassemblyDiagnoser(exportHtml: true)]
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

        [Benchmark]
        public RecordB PromoteUnsafe2()
        {
            return VeryUnsafe.ChangeType2<RecordB>(baseInstanceUnsafe);
        }

        [Benchmark]
        public RecordB PromoteUnsafe3()
        {
            return VeryUnsafe.ChangeType3<RecordB>(baseInstanceUnsafe);
        }


        public record RecordA(int A, int B)
        {
            public RecordB PromoteToB() => new(A, B);
        }
        public record RecordB(int A, int B) : RecordA(A, B);
    }
}
