using BenchmarkDotNet.Attributes;

namespace Danger.VeryUnsafe.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser(exportHtml: true)]
public class ObjectAllocation
{
    private Point4D allocated;

    [Benchmark(Baseline = true)]
    public void AllocateHeap()
    {
        allocated = new Point4D();
    }
    [Benchmark]
    public unsafe void StackAllocUnsafe()
    {
        int size = VeryUnsafe.GetObjectSize<Point4D>();
        byte* memory = stackalloc byte[size];
        var point = VeryUnsafe.InitializeObject<Point4D>(memory);

        // This extra line is to balance the extra assignment instruction in the equivalent heap allocation benchmark
        allocated = null;
    }

    public class Point4D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }
    }
}
