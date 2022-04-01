using NUnit.Framework;
using System;

namespace Danger.VeryUnsafe.Tests;

public class DelegateToActionTests
{
    public const string StructReturnIgnoreReason = "Methods returning structs are not currently supported.";

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

    [Test]
    public void InstanceLocalParameterizedClassReturnFuncToAction()
    {
        bool invoked = false;

        Func<int, PointC> f = Generator;
        var action = f.ToAction();

        // Verify that the delegate can be invoked as a Func<int>
        var result = f(5);
        Assert.That(invoked, Is.True);

        // Verify that the delegate can be invoked as an Action
        invoked = false;
        action(5);
        Assert.That(invoked, Is.True);

        PointC Generator(int all)
        {
            invoked = true;
            return new(all, all, all);
        }
    }

    [Test]
    [Ignore(StructReturnIgnoreReason)]
    public void InstanceLocalParameterlessStructReturnFuncToAction()
    {
        bool invoked = false;

        Func<Point> f = Generator;
        var action = f.ToAction();

        // Verify that the delegate can be invoked as a Func<int>
        var result = f();
        Assert.That(invoked, Is.True);

        // Verify that the delegate can be invoked as an Action
        invoked = false;
        action();
        Assert.That(invoked, Is.True);

        // At this point, if the stack has not spilled, we're good

        Point Generator()
        {
            invoked = true;
            return new();
        }
    }

    [Test]
    [Ignore(StructReturnIgnoreReason)]
    public void StaticLocalParameterlessStructReturnFuncToAction()
    {
        Func<Point> f = Generator;
        var action = f.ToAction();

        // Verify that the delegate can be invoked as a Func<int>
        var result = f();

        // Verify that the delegate can be invoked as an Action
        action();

        // At this point, if the stack has not spilled, we're good

        static Point Generator()
        {
            return new();
        }
    }

    [Test]
    [Ignore(StructReturnIgnoreReason)]
    public void InstanceLocalParameterizedStructReturnFuncToAction()
    {
        bool invoked = false;

        Func<int, Point> f = Generator;
        var action = f.ToAction();

        // Verify that the delegate can be invoked as a Func<int>
        var result = f(5);
        Assert.That(invoked, Is.True);

        // Verify that the delegate can be invoked as an Action
        invoked = false;
        action(5);
        Assert.That(invoked, Is.True);

        // At this point, if the stack has not spilled, we're good

        Point Generator(int all)
        {
            invoked = true;
            return new(all, all, all);
        }
    }

    private record struct Point(int X, int Y, int Z);
    private record class PointC(int X, int Y, int Z);
}
