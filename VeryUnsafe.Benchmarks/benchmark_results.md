|            Method |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD | Code Size |  Gen 0 | Allocated |
|------------------ |-----------:|----------:|----------:|-----------:|------:|--------:|----------:|-------:|----------:|
| PromoteViaCopying |  4.9755 ns | 0.1888 ns | 0.5416 ns |  4.9438 ns |  1.00 |    0.00 |      54 B | 0.0076 |      24 B |
|     PromoteUnsafe | 11.0430 ns | 0.3097 ns | 0.5175 ns | 11.0226 ns |  2.27 |    0.27 |      94 B |      - |         - |
|    PromoteUnsafe2 |  3.1825 ns | 0.1560 ns | 0.1669 ns |  3.1828 ns |  0.66 |    0.09 |      88 B |      - |         - |
|    PromoteUnsafe3 |  0.0616 ns | 0.0587 ns | 0.0948 ns |  0.0000 ns |  0.01 |    0.02 |      31 B |      - |         - |