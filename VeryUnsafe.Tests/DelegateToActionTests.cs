using NUnit.Framework;
using System;

namespace Danger.VeryUnsafe.Tests;

public class DelegateToActionTests
{
    [Test]
    public void StaticLocalFuncToAction()
    {
        const int expectedResult = 1;

        Func<int> f = Generator;
        var action = f.ToAction();

        // Verify that the delegate can be invoked as a Func<int>
        int result = f();
        Assert.AreEqual(expectedResult, result);

        // Verify that the delegate can be invoked as an Action
        action();
        // Unfortunately, since it's static, nothing can be tested

        static int Generator()
        {
            return expectedResult;
        }
    }

    [Test]
    public void InstanceLocalFuncToAction()
    {
        const int expectedResult = 1;

        bool invoked = false;

        Func<int> f = Generator;
        var action = f.ToAction();

        // Verify that the delegate can be invoked as a Func<int>
        int result = f();
        Assert.AreEqual(expectedResult, result);
        Assert.That(invoked, Is.True);

        // Verify that the delegate can be invoked as an Action
        invoked = false;
        action();
        Assert.That(invoked, Is.True);

        int Generator()
        {
            invoked = true;
            return expectedResult;
        }
    }
}
