using System;
using System.Runtime.CompilerServices;

namespace Danger.VeryUnsafe;

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

    /// <summary>Gets the size of an object type.</summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>The size that an object of type <typeparamref name="T"/> reserves.</returns>
    public static int GetObjectSize<T>()
        where T : class
    {
        return TypeHandles<T>.ObjectSize;
    }

    /// <summary>From allocated memory, prepares an object of the requested type. No constructor is called.</summary>
    /// <typeparam name="T">The type of the object to initialize.</typeparam>
    /// <param name="memory">The already allocated memory that the object will live in.</param>
    /// <returns>The instance of the object that is allocated in the block of memory that was provided in the method.</returns>
    /// <remarks>
    /// This method is intended to be called on a <see langword="stackalloc"/>'d block of memory.
    /// Otherwise, please, for the love of whomever you believe in, do a proper heap allocation.
    /// </remarks>
    public static T InitializeObject<T>(byte* memory)
        where T : class
    {
        *(nint*)memory = TypeHandles<T>.Handle;
        return Unsafe.AsRef<T>(&memory);
    }

    /// <summary>
    /// Gets the provided reference as a pointer of the provided type.
    /// </summary>
    /// <typeparam name="T">The type of the stored elements.</typeparam>
    /// <param name="reference">The reference to reinterpret as a pointer.</param>
    /// <returns>The provided reference reinterpreted as a pointer.</returns>
    public static T* ReferenceToPointer<T>(ref T reference)
    {
        fixed (T* ptr = &reference)
        {
            return ptr;
        }
    }

    private static class TypeHandles<T>
    {
        public static readonly nint Handle = typeof(T).TypeHandle.Value;

        public static readonly int ObjectSize;
        
        static TypeHandles()
        {
            // This is probably the hackiest thing in this entire project
            ObjectSize = ((int*)Handle)[1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeType(object obj)
        {
            nint* address = GetObjectHandleAddress(obj);
            *address = Handle;
        }
    }
}
