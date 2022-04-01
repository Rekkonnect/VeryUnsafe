using NUnit.Framework;

namespace Danger.VeryUnsafe.Tests;

public class TypeChangeTests
{
    [Test]
    public void ChangeType()
    {
        var instance = new C();
        Assert.AreEqual(3, instance.Property);

        VeryUnsafe.ChangeType<A>(instance);
        Assert.AreEqual(1, instance.Property);

        VeryUnsafe.ChangeType<B>(instance);
        Assert.AreEqual(2, instance.Property);

        Assert.True(instance.GetType() != typeof(C));
    }

    private class A
    {
        public virtual int Property => 1;
    }
    private class B : A
    {
        public override int Property => 2;
    }
    private class C : A
    {
        public override int Property => 3;
    }
}
