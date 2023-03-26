using System;
using System.Buffers;

namespace Danger.VeryUnsafe;

public unsafe class SequentialObjects<T>
{
    public T[] Objects { get; }
    public UnmanagedMemoryManager<byte> MemoryManager { get; }
    public Memory<byte> RawMemory => MemoryManager.Memory;

    public SequentialObjects(T[] objects, UnmanagedMemoryManager<byte> memory)
    {
        Objects = objects;
        MemoryManager = memory;
    }
}