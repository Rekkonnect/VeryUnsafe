namespace Danger.VeryUnsafe;

/// <summary>
/// A dangerous versionable collection.
/// </summary>
public interface IVersionableCollection
{
    /// <summary>
    /// A dangerously mutable version property.
    /// </summary>
    public int Version { get; set; }
}
