# VeryUnsafe
This library contains a collection of various dangerous operations. Using `unsafe` may be less than necessary in some cases, mind your steps.

## Disclaimer
Carefully read the documentation's remarks, for the operations are really unsafe in nature.

## Features
- `GetObjectHandleAddress`, to get the address of the type handle of an object
- `ChangeType`, to change the type handle of an object at runtime, without requiring reallocation

## Usages
- Pro-/demote objects at runtime
- Mimic another type (may or may not be heavily inspired by [Seb-stian/GetVoid](https://github.com/Seb-stian/GetVoid))
- Corrupt the managed heap in one more way

## Benchmarks
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

## To use or not to use
It's unsafe af. E. g. GC can move instances after we got the address but before we write to it. Or JIT doing some dirt that we broke the whole thing.

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

# Stay safe!
