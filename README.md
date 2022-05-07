# VeryUnsafe
This library contains a collection of various dangerous operations. Using `unsafe` may be less than necessary in some cases, mind your steps.

## Disclaimer
Carefully read the documentation's remarks, for the operations are really unsafe in nature.

## Features
- `GetObjectHandleAddress`, to get the address of the type handle of an object
- `ChangeType`, to change the type handle of an object at runtime, without requiring reallocation
- `ToAction` extensions for `Func` delegates, using `ChangeType`
- `GetObjectSize`, to get the allocated memory size of an object
- `InitializeObject`, to prepare the given block of memory for being used as an object, and get a reference to it

## Usages
- Pro-/demote objects at runtime
- Mimic another type (may or may not be heavily inspired by [Seb-stian/GetVoid](https://github.com/Seb-stian/GetVoid))
- Discard the return type of a delegate instance (inspired by a real world usage)
- Allocate a reference type object using `stackalloc`
- Corrupt the managed heap in one more way

### Allocate a reference type object using `stackalloc`? Are you crazy?

Yes, I'm very crazy, and this is how you can be too:
```csharp
int size = VeryUnsafe.GetObjectSize<Point4D>();
byte* memory = stackalloc byte[size];
var point = VeryUnsafe.InitializeObject<Point4D>(memory);
```

No constructor is called, the memory is just allocated and reserved for your stack-living object.

## Benchmarks
### Changing Object Type
It's fast. Hella fast.
```assembly
push      rax
mov       rax,[rcx+10]
mov       [rsp],rax
mov       rdx,[rsp]
mov       rcx,offset MT_VeryUnsafe.Benchmarks.ObjectPromotion+RecordB
mov       [rdx],rcx
add       rsp,8
ret
```

And, well, here are the results of a [reference benchmark](VeryUnsafe.Benchmarks/ObjectPromotion.cs):

|            Method |      Mean |     Error |    StdDev |    Median | Ratio |  Gen 0 | Code Size | Allocated |
|------------------ |----------:|----------:|----------:|----------:|------:|-------:|----------:|----------:|
| PromoteViaCopying | 4.4002 ns | 0.1016 ns | 0.0900 ns | 4.3919 ns | 1.000 | 0.0057 |      54 B |      24 B |
|     PromoteUnsafe | 0.0022 ns | 0.0066 ns | 0.0059 ns | 0.0000 ns | 0.001 |      - |      31 B |         - |
> ObjectPromotion.PromoteUnsafe: Default -> The method duration is indistinguishable from the empty method duration

### Allocating
You hear everywhere that object allocations are costly. **Heap** allocations, that is. And here is proof from a [reference benchmark](VeryUnsafe.Benchmarks/ObjectAllocation.cs):

|           Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 | Code Size | Allocated |
|----------------- |---------:|----------:|----------:|------:|-------:|----------:|----------:|
|     AllocateHeap | 4.494 ns | 0.1228 ns | 0.3340 ns |  1.00 | 0.0077 |      42 B |      32 B |
| StackAllocUnsafe | 1.038 ns | 0.0132 ns | 0.0124 ns |  0.22 |      - |      89 B |         - |

## To use or not to use
Everything here is unsafe af. E. g. GC can move instances after we got the address but before we write to it. Or JIT doing some dirt that we broke the whole thing.

## Some constructive feedback

> Don't use it. (c) WhiteBlackGoose

> I hate it (c) WhiteBlackGoose

<hr>

> what I don't understand is why go further to try and make it "safer"
because that can't happen (c) Tanner Gooding

> there is no appeal, you can't make it safer (c) Tanner Gooding

> its a runtime corrupting feature, so it can't be safe  (c) Tanner Gooding

> there is no "safer" (c) Tanner Gooding

> that won't make this safer, you are still risking JIT corruption by lying about the type (c) Tanner Gooding

> there is no real difference between "shoot yourself in the bare foot" and "shoot yourself in the foot with a sock on" (c) Tanner Gooding

<hr>

> lgtm, can't wait to use it (c) Sebastian

> Honestly I prefer my unsafe code safe<br>
> But I also love micro-optimizations, so it's hard for me to decide whether I absolutely love it or despise it (c) Sebastian

<hr>

> Thanks, I hate it :laughing: (c) Sergio

<hr>

> https://tenor.com/view/exorcism-father-mulvehill-evil-z-is-for-zombies-get-out-evil-spirit-gif-22407195 (c) Fred

# Stay safe!
