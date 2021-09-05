# VeryUnsafe (don't use it)
# Don't use it
Make sure not to use it.

## Disclaimer
Don't use it, like really.

## Don't use it
Make sure not to use it, like, forget.

## Usages
There's no usages, lol, don't use it!

## Benchmarks
But it's fast. Not using it means that you don't get casting as fast as
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
Not that you care, since you don't use it

## Why not to use it
Because why on Earth use it, it's unsafe af. E. g. GC can move instances after we got the address but before we write to it. Or JIT doing some dirt that we broke the whole thing.

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
