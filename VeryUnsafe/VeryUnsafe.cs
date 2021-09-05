using System;
using System.Runtime.CompilerServices;

namespace VeryUnsafe
{
    /// <summary>Provides a collection of dangerous memory operations, some requiring usage of <see langword="unsafe"/>.</summary>
    public static unsafe class VeryUnsafe
    {
        /// <summary>Gets the object's type handle value address.</summary>
        /// <param name="obj">The object instance whose type handle to get.</param>
        /// <returns>The address of the object's type handle value.</returns>
        public static nint* GetObjectHandleAddress(object obj)
        {
            return *(nint**)Unsafe.AsPointer(ref obj);
        }

        /// <inheritdoc cref="ChangeType(object, Type)"/>
        /// <typeparam name="T">The new type of the object.</typeparam>
        public static void ChangeType<T>(object obj) => ChangeType(obj, typeof(T));

        /// <summary>Changes the type of the object by adjusting its type handle.</summary>
        /// <param name="obj">The object instance whose type to change.</param>
        /// <param name="newType">The new type of the object.</param>
        /// <remarks>
        /// There are no validation checks performed regarding the type compatibility. The provided object's type will be considered the new one and it will behave accordingly.
        /// For types of different sizes, memory corruption issues may arise. For safer results, ensure that this operation is performed on types of same size.
        /// </remarks>
        public static void ChangeType(object obj, Type newType)
        {
            nint* address = GetObjectHandleAddress(obj);
            *address = newType.TypeHandle.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChangeType2<T>(object obj)
        {
            ChangeType2(obj, typeof(T));
            return Unsafe.As<object, T>(ref obj);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeType2(object obj, Type newType)
        {
            nint* address = GetObjectHandleAddress2(obj);
            *address = newType.TypeHandle.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint* GetObjectHandleAddress2(object obj)
        {
            return (nint*)Unsafe.As<object, nint>(ref obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChangeType3<T>(object obj)
        {
            Quack<T>.ChangeType3(obj);
            return Unsafe.As<object, T>(ref obj);
        }

        private static class Quack<T>
        {
            private static readonly IntPtr thingy = typeof(T).TypeHandle.Value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ChangeType3(object obj)
            {
                nint* address = GetObjectHandleAddress3(obj);
                *address = thingy;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static nint* GetObjectHandleAddress3(object obj)
            {
                return (nint*)Unsafe.As<object, nint>(ref obj);
            }
        }
    }
}
