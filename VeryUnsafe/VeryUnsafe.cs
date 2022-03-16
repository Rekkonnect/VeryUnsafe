using System;
using System.Runtime.CompilerServices;

namespace VeryUnsafe;

/// <summary>Provides a collection of dangerous memory operations, some requiring usage of <see langword="unsafe"/>.</summary>
public static unsafe partial class VeryUnsafe
{
    /// <summary>Gets the object's type handle value address.</summary>
    /// <param name="obj">The object instance whose type handle to get.</param>
    /// <returns>The address of the object's type handle value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint* GetObjectHandleAddress(object obj)
    {
        return *(nint**)Unsafe.AsPointer(ref obj);
    }

    /// <inheritdoc cref="ChangeType(object, Type)"/>
    /// <typeparam name="T">The new type of the object.</typeparam>
    /// <returns>The same instance of the object as the new type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ChangeType<T>(object obj)
    {
        TypeHandles<T>.ChangeType(obj);
        return Unsafe.As<object, T>(ref obj);
    }

    /// <summary>Changes the type of the object by adjusting its type handle.</summary>
    /// <param name="obj">The object instance whose type to change.</param>
    /// <param name="newType">The new type of the object.</param>
    /// <remarks>
    /// There are no validation checks performed regarding the type compatibility. The provided object's type will be considered the new one and it will behave accordingly.
    /// For types of different sizes, memory corruption issues may arise. For safer results, ensure that this operation is performed on types of same size.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ChangeType(object obj, Type newType)
    {
        nint* address = GetObjectHandleAddress(obj);
        *address = newType.TypeHandle.Value;
    }

    private static class TypeHandles<T>
    {
        private static readonly nint handle = typeof(T).TypeHandle.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeType(object obj)
        {
            nint* address = GetObjectHandleAddress(obj);
            *address = handle;
        }
    }
}
