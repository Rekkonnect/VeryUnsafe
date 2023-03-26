using Danger.VeryUnsafe;

namespace UnreliablyDangerous;

#nullable enable

public static unsafe class UnsafeAllocations
{
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
    public static SequentialObjects<T> SequentialObjects<T>(int count)
        where T : class
    {
        int size = VeryUnsafe.GetObjectSize<T>() + 2 * sizeof(nint);
        int byteSize = size * count;
        var memoryManager = UnmanagedMemoryManager<byte>.AllocateHGlobal(byteSize);
        var objectMemoryPointer = memoryManager.Pointer;

        var objectArray = new T[count];
        T* objectBeginning = (T*)objectMemoryPointer;

        fixed (T* objectArrayPointer = objectArray)
        {
            var pointerArray = (T**)objectArrayPointer;

            for (int i = 0; i < count; i++)
            {
                byte* objectPointer = objectMemoryPointer + i * size;
                VeryUnsafe.InitializeObject<T>(objectPointer);
                pointerArray[i] = (T*)objectPointer;
            }
        }

        return new(objectArray, memoryManager);
    }
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
}
