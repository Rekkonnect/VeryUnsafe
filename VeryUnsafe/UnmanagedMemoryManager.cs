using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace Danger.VeryUnsafe;

/// <summary>
/// A <seealso cref="MemoryManager{T}"/> over a raw pointer.
/// </summary>
/// <remarks>
/// The pointer is assumed to be fully unmanaged,
/// or externally pinned; no attempt will be made to pin this data.
/// </remarks>
public sealed unsafe class UnmanagedMemoryManager<T> : MemoryManager<T>
    where T : unmanaged
{
    public T* Pointer { get; }
    public int Length { get; }

    /// <summary>
    /// Gets the claimed allocation source of the memory.
    /// This property is used to determine where the method
    /// of freeing the memory, if the allocation source is
    /// known and there is an explicit memory freeing method.
    /// </summary>
    public AllocationSource AllocationSource { get; }

    /// <summary>
    /// Create a new <seealso cref="UnmanagedMemoryManager{T}"/>
    /// instance from a <seealso cref="Span{T}"/>.
    /// </summary>
    /// <remarks>
    /// It is assumed that the span provided is already unmanaged
    /// or externally pinned.
    /// </remarks>
    public UnmanagedMemoryManager(Span<T> span, AllocationSource allocationSource = AllocationSource.Unknown)
    {
        Pointer = VeryUnsafe.ReferenceToPointer(ref MemoryMarshal.GetReference(span));
        Length = span.Length;
        AllocationSource = allocationSource;
    }
    /// <summary>
    /// Create a new <seealso cref="UnmanagedMemoryManager{T}"/>
    /// instance from a pointer and size.
    /// </summary>
    /// <param name="length">The number of elements of the type.</param>
    /// <param name="pointer">The pointer to the first element.</param>
    /// <param name="allocationSource">
    /// The allocation source from which the pointer is valid.
    /// Provide <seealso cref="AllocationSource.Unknown"/> for
    /// unknowningly-provided allocation sources.
    /// </param>
    public UnmanagedMemoryManager(T* pointer, int length, AllocationSource allocationSource = AllocationSource.Unknown)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        Pointer = pointer;
        Length = length;
        AllocationSource = allocationSource;
    }

    /// <summary>
    /// Obtains a span that represents the region.
    /// </summary>
    public override Span<T> GetSpan() => new(Pointer, Length);

    /// <summary>
    /// Provides access to a pointer that represents the data.
    /// The memory is not actually pinned.
    /// </summary>
    public override MemoryHandle Pin(int elementIndex = 0)
    {
        if (elementIndex < 0 || elementIndex >= Length)
            throw new ArgumentOutOfRangeException(nameof(elementIndex));
        return new(Pointer + elementIndex);
    }

    /// <summary>
    /// Does nothing, based on the assumptions of being unmanaged
    /// or externally pinned from other sources.
    /// </summary>
    public override void Unpin() { }

    /// <summary>
    /// Does nothing. To dispose the memory, manually
    /// free it from the source that it was allocated.
    /// </summary>
    /// <remarks>
    /// Consider using <seealso cref="FreeHGlobal"/>,
    /// <seealso cref="FreeCoTaskMem"/> or
    /// <seealso cref="FreeBSTR"/>.
    /// The <seealso cref="AllocationSource"/> is not accounted
    /// for, to rely on this proeprty use
    /// </remarks>
    protected override void Dispose(bool disposing) { }

    /// <summary>
    /// Attempts to free the allocated memory from the claimed
    /// allocation source, taking the <seealso cref="AllocationSource"/>
    /// property into account.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the allocation source is known
    /// and allows manual freeing of the allocated memory,
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool FreeFromClaimedSource()
    {
        return VariantMarshal.Free(Pointer, AllocationSource);
    }
    /// <summary>
    /// Frees the memory using
    /// <seealso cref="Marshal.FreeHGlobal(IntPtr)"/>.
    /// </summary>
    public void FreeHGlobal()
    {
        Marshal.FreeHGlobal((IntPtr)Pointer);
    }
    /// <summary>
    /// Frees the memory using
    /// <seealso cref="Marshal.FreeCoTaskMem(IntPtr)"/>.
    /// </summary>
    public void FreeCoTaskMem()
    {
        Marshal.FreeCoTaskMem((IntPtr)Pointer);
    }
    /// <summary>
    /// Frees the memory using
    /// <seealso cref="Marshal.FreeBSTR(IntPtr)"/>.
    /// </summary>
    public void FreeBSTR()
    {
        Marshal.FreeBSTR((IntPtr)Pointer);
    }

    public static UnmanagedMemoryManager<T> AllocateHGlobal(int count)
    {
        var memory = (T*)Marshal.AllocHGlobal(count);
        return new(memory, count, AllocationSource.HGlobal);
    }
    public static UnmanagedMemoryManager<T> AllocateCoTaskMem(int count)
    {
        var memory = (T*)Marshal.AllocCoTaskMem(count);
        return new(memory, count, AllocationSource.CoTaskMem);
    }
}
