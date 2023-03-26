using System.Runtime.InteropServices;

namespace Danger.VeryUnsafe;

/// <summary>
/// Defines the various available memory allocation sources.
/// </summary>
public enum AllocationSource
{
    /// <summary>
    /// Reflects that the allocation source is not known.
    /// </summary>
    Unknown,
    
    /// <summary>
    /// Reflects that the allocation source is the managed
    /// memory controller from the CLR.
    /// </summary>
    Managed,

    /// <summary>
    /// Reflects that the memory was allocated to the unmanaged
    /// memory heap using an allocation method like
    /// <seealso cref="Marshal.AllocHGlobal(int)"/>.
    /// </summary>
    HGlobal,
    /// <summary>
    /// Reflects that the memory was allocated to the unmanaged
    /// memory heap using an allocation method like
    /// <seealso cref="Marshal.AllocCoTaskMem(int)"/>.
    /// </summary>
    CoTaskMem,
    /// <summary>
    /// Reflects that the memory was allocated to the unmanaged
    /// memory heap using an allocation method like
    /// <seealso cref="Marshal.SecureStringToBSTR(System.Security.SecureString)"/>.
    /// </summary>
    BSTR,
}
