using NUnit.Framework;

namespace Danger.VeryUnsafe.Tests;

public unsafe class StackAllocObjectTests
{
    [Test]
    public void StackAllocUse()
    {
        var more = new MoreValues();

        int size = VeryUnsafe.GetObjectSize<Values>();
        byte* memory = stackalloc byte[size];
        var values = VeryUnsafe.InitializeObject<Values>(memory);
        
        values.A = 1;
        values.B = 2;
        values.C = 3;
        
        more.AssignFromValues(values);

        Assert.AreEqual(1, more.A);
        Assert.AreEqual(2, more.B);
        Assert.AreEqual(3, more.C);
    }
    [Test]
    public void StackAllocWith()
    {
        int size = VeryUnsafe.GetObjectSize<Point>();
        byte* memory = stackalloc byte[size];
        var point = VeryUnsafe.InitializeObject<Point>(memory);

        int previousY = point.Y;
        int previousZ = point.Z;
        int previousW = point.W;

        point = point with { X = 2 };
        Assert.AreEqual(2, point.X);
        Assert.AreEqual(previousY, point.Y);
        Assert.AreEqual(previousZ, point.Z);
        Assert.AreEqual(previousW, point.W);
    }

    private record Point(int X, int Y, int Z, int W);

    private class Values
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }
    private class MoreValues
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }

        public void AssignFromValues(Values values)
        {
            A = values.A;
            B = values.B;
            C = values.C;
        }
    }
}
