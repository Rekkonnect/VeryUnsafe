using System;
using System.Collections.Generic;

namespace Danger.VeryUnsafe;

/// <summary>
/// Represents an unsafe list that uses the same memory layout as
/// <seealso cref="List{T}"/> allowing unsafe mutations and adjustments
/// quickly, efficiently and statically.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// If manually constructed through the constructors, do not forget to
/// use <seealso cref="VeryUnsafe.ChangeType{T}(object)"/> to cast back
/// to a list.
/// </remarks>
public class UnsafeList<T> : ICountableCollection, IVersionableCollection
{
    #region Fields
    // Using backing fields directly to guarantee order of the fields
    // so that reinterpretation of the memory does not break

#pragma warning disable IDE0032 // Use auto property
    private T[] items;
    private int count;
    private int version;
#pragma warning restore IDE0032 // Use auto property
    #endregion

    public T[] Items
    {
        get => items;
        set => items = value;
    }
    /// <inheritdoc/>
    public int Count
    {
        get => count;
        set => count = value;
    }

    /// <summary>
    /// Gets or sets the version of the collection.
    /// This value is used to indicate the mutations that the
    /// list has undergone, and is evaluated during an active
    /// enumerator.
    /// </summary>
    public int Version
    {
        get => version;
        set => version = value;
    }
    /// <summary>
    /// Gets the capacity of the list from the underlying array
    /// in <seealso cref="Items"/>.
    /// </summary>
    /// <remarks>
    /// To change the capacity, use <seealso cref="ReallocateArray(int)"/>.
    /// </remarks>
    public int Capacity => Items.Length;

    public UnsafeList() { }
    public UnsafeList(T[] array, int count)
    {
        items = array;
        this.count = count;
    }

    /// <inheritdoc cref="ReallocateArray(int, out T[])"/>
    public void ReallocateArray(int newCapacity)
    {
        ReallocateArray(newCapacity, out _);
    }
    /// <summary>
    /// Reallocates the underlying array into a new one with
    /// the specified capacity.
    /// </summary>
    /// <param name="newCapacity">
    /// The new capacity of the list, which is the length of the newly
    /// allocated array.
    /// </param>
    /// <param name="oldArray">
    /// Gets the underlying array that was being used before the
    /// reallocation.
    /// </param>
    public void ReallocateArray(int newCapacity, out T[] oldArray)
    {
        oldArray = Items;
        var newArray = new T[newCapacity];
        Items.CopyTo(newArray.AsSpan());
        Items = newArray;
    }

    /// <summary>
    /// Increments the <seealso cref="Version"/> property to indicate
    /// that a change has occurred.
    /// </summary>
    public void IncrementVersion()
    {
        Version++;
    }

    /// <summary>
    /// Resizes the list into the specified count.
    /// </summary>
    /// <param name="newCount">The new count of the list.</param>
    /// <remarks>
    /// The underlying array will be reallocated into a new one if
    /// the new count exceeds the current capacity of the list.
    /// The newly allocated array will have the minimum capacity
    /// necessary to meet the new count, meaning there will be no
    /// unused elements.<br/>
    /// Otherwise, the count of the list is set to the new value,
    /// preserving the original array and its contents. This offers
    /// a quick and performant solution to trimming the contents of
    /// the list, and enabling replacing those old contents in the
    /// future.
    /// </remarks>
    public void Resize(int newCount)
    {
        if (newCount > Capacity)
        {
            ReallocateArray(newCount);
        }
        Count = newCount;
        IncrementVersion();
    }

    /// <summary>
    /// Retypes this object into a safe <seealso cref="List{T}"/>.
    /// </summary>
    /// <returns>
    /// This object that will have been retyped into a
    /// <seealso cref="List{T}"/>.
    /// </returns>
    /// <remarks>
    /// This object is no longer an <seealso cref="UnsafeList{T}"/>
    /// after invoking this method. However, retyping it back to an
    /// <seealso cref="UnsafeList{T}"/> will be unnecessary, and it
    /// is recommended to access the list unsafely through the
    /// <seealso cref="UnsafeCollections.AsUnsafeList{T}(List{T})"/>
    /// extension.
    /// </remarks>
    public List<T> RetypeToSafe()
    {
        return VeryUnsafe.ChangeType<List<T>>(this);
    }
}
