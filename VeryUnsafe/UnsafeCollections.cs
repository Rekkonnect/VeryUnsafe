using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Danger.VeryUnsafe;

/// <summary>
/// Provides unsafe operations about collections.
/// </summary>
public static unsafe class UnsafeCollections
{
    /// <summary>
    /// Reinterprets an <seealso cref="UnsafeList{T}"/> into a
    /// <seealso cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the stored elements.</typeparam>
    /// <param name="list">The unsafe list to reinterpret.</param>
    /// <returns>
    /// The provided <seealso cref="UnsafeList{T}"/>
    /// reinterpreted as a <seealso cref="List{T}"/>.
    /// </returns>
    public static List<T> AsSafeList<T>(this UnsafeList<T> list)
    {
        return Unsafe.As<UnsafeList<T>, List<T>>(ref list);
    }
    /// <summary>
    /// Reinterprets an <seealso cref="List{T}"/> into a
    /// <seealso cref="UnsafeList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the stored elements.</typeparam>
    /// <param name="list">The list to reinterpret.</param>
    /// <returns>
    /// The provided <seealso cref="List{T}"/> reinterpreted
    /// as a <seealso cref="UnsafeList{T}"/>.
    /// </returns>
    public static UnsafeList<T> AsUnsafeList<T>(this List<T> list)
    {
        return Unsafe.As<List<T>, UnsafeList<T>>(ref list);
    }

    /// <inheritdoc cref="UnsafeList{T}.Resize(int)"/>
    public static void Resize<T>(this List<T> list, int newCount)
    {
        list.AsUnsafeList().Resize(newCount);
    }

    /// <summary>
    /// Initializes a new <seealso cref="List{T}"/> with a size
    /// and an implicitly created underlying array of the specified
    /// size, with all items initialized to <see langword="default"/>.
    /// The list is directly accessible at the indices within the
    /// specified size.
    /// </summary>
    /// <typeparam name="T">The type of the stored elements.</typeparam>
    /// <param name="count">The size of the list.</param>
    /// <returns>The initialized <seealso cref="List{T}"/>.</returns>
    public static List<T> ListWithDefaultItems<T>(int count)
    {
        return ListWithUnderlyingArray(new T[count]);
    }
    /// <summary>
    /// Initializes a new <seealso cref="List{T}"/> from a
    /// given underlying array and the count equal to the array's
    /// size.
    /// </summary>
    /// <typeparam name="T">The type of the stored elements.</typeparam>
    /// <param name="array">The underlying array to be used.</param>
    /// <returns>The initialized <seealso cref="List{T}"/>.</returns>
    public static List<T> ListWithUnderlyingArray<T>(T[] array)
    {
        return ListWithUnderlyingArray(array, array.Length);
    }
    /// <summary>
    /// Initializes a new <seealso cref="List{T}"/> from a
    /// given underlying array and the count equal to the array's
    /// size.
    /// </summary>
    /// <typeparam name="T">The type of the stored elements.</typeparam>
    /// <param name="array">The underlying array to be used.</param>
    /// <param name="count">The size of the list.</param>
    /// <returns>The initialized <seealso cref="List{T}"/>.</returns>
    public static List<T> ListWithUnderlyingArray<T>(T[] array, int count)
    {
        var list = new List<T>();
        var unsafeList = list.AsUnsafeList();
        unsafeList.Items = array;
        unsafeList.Count = count;
        return list;
    }
}
