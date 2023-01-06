using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Danger.VeryUnsafe.Tests;

public class UnsafeCollectionsTests
{
    [Test]
    public void ListWithDefaultItems()
    {
        const int originalCount = 1563;

        var list = UnsafeCollections.ListWithDefaultItems<string>(originalCount);
        list.Should().HaveCount(originalCount);
        list.Should().OnlyContain(e => e == null);
    }
    [Test]
    public void ListWithUnderlyingArray()
    {
        const int originalCount = 1563;

        var underlyingArray = Enumerable.Range(0, originalCount).ToArray();
        var list = UnsafeCollections.ListWithUnderlyingArray(underlyingArray);

        CollectionAssert.AreEqual(underlyingArray, list);

        const int shorterCount = 134;
        var shorterList = UnsafeCollections.ListWithUnderlyingArray(underlyingArray, shorterCount);
        var shorterUnderlyingArray = underlyingArray[..shorterCount];
        CollectionAssert.AreEqual(shorterUnderlyingArray, shorterList);
    }
    [Test]
    public void Resize()
    {
        const int originalCount = 412;
        var list = Enumerable.Range(1, originalCount).ToList();

        const int shorterCount = 13;
        list.Resize(shorterCount);
        list.Should().HaveCount(shorterCount);
        Assert.Catch(() => _ = list[shorterCount]);

        // The original array is preserved and thus its old elements
        // are still accessible through the underlying array
        const int largerCount = 4841;
        list.Resize(largerCount);
        list.Should().HaveCount(largerCount);

        for (int i = 0; i < originalCount; i++)
        {
            Assert.True(list[i] > 0);
        }
        for (int i = originalCount; i < largerCount; i++)
        {
            Assert.True(list[i] == default);
        }
    }
}
