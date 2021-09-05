## .NET 5.0.8 (5.0.821.31504), X64 RyuJIT
```assembly
; VeryUnsafe.Benchmarks.ObjectPromotion.PromoteViaCopying()
       mov       rcx,[rcx+8]
       cmp       [rcx],ecx
       jmp       near ptr VeryUnsafe.Benchmarks.ObjectPromotion+RecordA.PromoteToB()
; Total bytes of code 11
```
```assembly
; VeryUnsafe.Benchmarks.ObjectPromotion+RecordA.PromoteToB()
       push      rdi
       push      rsi
       sub       rsp,28
       mov       rsi,rcx
       mov       edi,[rsi+8]
       mov       rcx,offset MT_VeryUnsafe.Benchmarks.ObjectPromotion+RecordB
       call      CORINFO_HELP_NEWSFAST
       mov       edx,[rsi+0C]
       mov       [rax+8],edi
       mov       [rax+0C],edx
       add       rsp,28
       pop       rsi
       pop       rdi
       ret
; Total bytes of code 43
```

## .NET 5.0.8 (5.0.821.31504), X64 RyuJIT
```assembly
; VeryUnsafe.Benchmarks.ObjectPromotion.PromoteUnsafe()
       push      rsi
       sub       rsp,20
       mov       rsi,rcx
       mov       rdx,[rsi+10]
       mov       rcx,offset MD_VeryUnsafe.VeryUnsafe.ChangeType(System.Object)
       call      VeryUnsafe.VeryUnsafe.ChangeType[[System.__Canon, System.Private.CoreLib]](System.Object)
       mov       rdx,[rsi+10]
       mov       rcx,offset MT_VeryUnsafe.Benchmarks.ObjectPromotion+RecordB
       call      CORINFO_HELP_ISINSTANCEOFCLASS
       nop
       add       rsp,20
       pop       rsi
       ret
; Total bytes of code 53
```
```assembly
; VeryUnsafe.VeryUnsafe.ChangeType[[System.__Canon, System.Private.CoreLib]](System.Object)
       push      rsi
       sub       rsp,30
       mov       [rsp+28],rcx
       mov       rsi,rdx
       mov       rcx,[rcx+10]
       mov       rcx,[rcx]
       call      CORINFO_HELP_TYPEHANDLE_TO_RUNTIMETYPE
       mov       rdx,rax
       mov       rcx,rsi
       add       rsp,30
       pop       rsi
       jmp       near ptr VeryUnsafe.VeryUnsafe.ChangeType(System.Object, System.Type)
; Total bytes of code 41
```

## .NET 5.0.8 (5.0.821.31504), X64 RyuJIT
```assembly
; VeryUnsafe.Benchmarks.ObjectPromotion.PromoteUnsafe2()
       push      rdi
       push      rsi
       sub       rsp,38
       xor       eax,eax
       mov       [rsp+30],rax
       mov       rsi,[rcx+10]
       mov       rcx,offset MT_VeryUnsafe.Benchmarks.ObjectPromotion+RecordB
       call      CORINFO_HELP_TYPEHANDLE_TO_RUNTIMETYPE
       mov       [rsp+28],rsi
       mov       rdi,[rsp+28]
       cmp       [rax],eax
       mov       [rsp+30],rax
       lea       rcx,[rsp+30]
       call      System.RuntimeTypeHandle.get_Value()
       mov       [rdi],rax
       mov       rax,rsi
       add       rsp,38
       pop       rsi
       pop       rdi
       ret
; Total bytes of code 72
```
```assembly
; System.RuntimeTypeHandle.get_Value()
       mov       rdx,[rcx]
       test      rdx,rdx
       jne       short M01_L00
       xor       eax,eax
       ret
M01_L00:
       mov       rax,[rdx+18]
       ret
; Total bytes of code 16
```

## .NET 5.0.8 (5.0.821.31504), X64 RyuJIT
```assembly
; VeryUnsafe.Benchmarks.ObjectPromotion.PromoteUnsafe3()
       push      rax
       mov       rax,[rcx+10]
       mov       [rsp],rax
       mov       rdx,[rsp]
       mov       rcx,offset MT_VeryUnsafe.Benchmarks.ObjectPromotion+RecordB
       mov       [rdx],rcx
       add       rsp,8
       ret
; Total bytes of code 31
```

