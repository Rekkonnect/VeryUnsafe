using System;
using System.Runtime.InteropServices;

namespace Danger.VeryUnsafe;

/// <summary>
/// Provides additional handy methods for <seealso cref="Marshal"/>.
/// </summary>
public static unsafe class VariantMarshal
{
    /// <inheritdoc cref="Free(IntPtr, AllocationSource)"/>
    public static bool Free(void* ptr, AllocationSource source)
    {
        return Free((IntPtr)ptr, source);
    }
    /// <summary>
    /// Attempts to free the memory at the speciified location,
    /// based on the claimed allocation source of the memory.
    /// </summary>
    /// <param name="ptr">The pointer to free.</param>
    /// <param name="source">
    /// The claimed allocation source of the memory. The corresponding
    /// freeing method from the <seealso cref="Marshal"/> class will
    /// be called depending on the allocation source.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the allocation source supports
    /// manually freeing the memory, i.e. it is equal to any of
    /// the following:
    /// <list type="bullet">
    /// <item><seealso cref="AllocationSource.HGlobal"/></item>
    /// <item><seealso cref="AllocationSource.CoTaskMem"/></item>
    /// <item><seealso cref="AllocationSource.BSTR"/></item>
    /// </list>
    /// Otherwise, <see langword="false"/> is returned.
    /// </returns>
    /// <remarks>
    /// This method will do nothing if the allocation source is
    /// not any of the values that return <see langword="true"/>.
    /// As a result, it is safe to invoke this method on scenarios
    /// with parameterized allocation sources.
    /// <br/>
    /// Any exceptions being thrown during the process of freeing
    /// the memory are not handled.
    /// </remarks>
    public static bool Free(IntPtr ptr, AllocationSource source)
    {
        switch (source)
        {
            case AllocationSource.HGlobal:
                Marshal.FreeHGlobal(ptr);
                return true;

            case AllocationSource.CoTaskMem:
                Marshal.FreeCoTaskMem(ptr);
                return true;

            case AllocationSource.BSTR:
                Marshal.FreeBSTR(ptr);
                return true;

            default:
                return false;
        }
    }
}
