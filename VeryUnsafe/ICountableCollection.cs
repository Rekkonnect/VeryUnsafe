namespace Danger.VeryUnsafe;

/// <summary>
/// A dangerous countable collection.
/// </summary>
public interface ICountableCollection
{
    /// <summary>
    /// A dangerously mutable count property.
    /// </summary>
    public int Count { get; set; }
}
