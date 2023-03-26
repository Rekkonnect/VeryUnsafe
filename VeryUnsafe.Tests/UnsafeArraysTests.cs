using NUnit.Framework;
using System;
using UnreliablyDangerous;

namespace Danger.VeryUnsafe.Tests;

public class UnsafeArraysTests
{
    [Test]
    public void SequentialObjects()
    {
        const int objectCount = 8342;
        var objects = UnsafeAllocations.SequentialObjects<CustomArguments>(objectCount);
        var expectedObjects = new CustomArguments[objectCount];
        for (int i = 0; i < objects.Objects.Length; i++)
        {
            MutateArrayObject(objects.Objects);

            expectedObjects[i] = new();
            MutateArrayObject(expectedObjects);

            // Sometimes there occurs an AV on the string assignment
            void MutateArrayObject(CustomArguments[] array)
            {
                var arguments = array[i];
                arguments.MyArgument = i + 42;
                arguments.Age = i;
                arguments.Name = $"Index {i}";
                arguments.Average = i / 314.3d;
                arguments.AnotherArgument = i * i;
            }
        }

        int equals = 0;

        for (int i = 0; i < objectCount; i++)
        {
            bool equal = expectedObjects[i].Equals(objects.Objects[i]);
            equals += equal ? 1 : 0;
        }

        objects.MemoryManager.FreeFromClaimedSource();

        // Often times this fails with a 98% ratio on the equal objects
        // And sometimes it might fail with about 4000 as the equal objects
        Assert.AreEqual(objectCount, equals);
    }

    private class CustomArguments
    {
        public int MyArgument { get; set; }
        public int AnotherArgument { get; set; }
        public int Age { get; set; }
        public int Size { get; set; }
        public double Length { get; set; }
        public int Dimensions { get; set; }
        public double Average { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CustomArguments arguments &&
                   MyArgument == arguments.MyArgument &&
                   AnotherArgument == arguments.AnotherArgument &&
                   Age == arguments.Age &&
                   Size == arguments.Size &&
                   Length == arguments.Length &&
                   Dimensions == arguments.Dimensions &&
                   Average == arguments.Average &&
                   Name == arguments.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MyArgument, AnotherArgument, Age, Size, Length, Dimensions, Average, Name);
        }
    }
}
