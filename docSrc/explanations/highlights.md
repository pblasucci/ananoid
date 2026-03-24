---
title: Performance Highlights
category: Explanations
categoryindex: 2
index: 2
---

Performance: Select Highlights
===

While ananoid is more concerned with correctness and ergonomics, it nevertheless trie to maintain good performance.
To that end, the repository includes a project which benchmarks several different aspects of the library, via the
excellent [Benchmark.NET][1] library. As with all performance assessments, the results should be taken with a grain of
salt, and are not necessarily indicative of every possible situation wherein ananoid might be used. Consumers should
always profile their own use cases, and not rely solely on the results presented here. That being said, what follows
are some important highlights.

### Highlight: Ananoid vs NanoidNet vs Guid

Perhaps the most intersting results are those comparing ananoid to the popular [NanoidNet][2] library (and to the
built-in `Guid` type, as a baseline). Each candidate is invoked numerous times, using its respective default settings,
to track the general throughput, memory usage, and other relevant metrics.

<div class="perf">

| Method    | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Guid      |  63.66 ns | 0.478 ns | 0.424 ns |  1.00 |    0.01 | 0.0102 |      96 B |        1.00 |
| NanoidNet | 125.05 ns | 2.032 ns | 1.586 ns |  1.96 |    0.03 | 0.0212 |     200 B |        2.08 |
| Ananoid   |  86.77 ns | 0.361 ns | 0.320 ns |  1.36 |    0.01 | 0.0136 |     128 B |        1.33 |

</div>

### Highlight: Structs vs Functions

Ananoid is designed to have both a low-level API, working in terms of primitives. And a high-level
API, wherein everything is driven by bespoke types. It is important to note how the high-level API
improves correctness with only minimal performance overhead, as shown in the following comparison of the two approaches.

<div class="perf">

| Method   | Mean     | Error    | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|--------- |---------:|---------:|---------:|------:|-------:|----------:|------------:|
| Function | 89.31 ns | 0.504 ns | 0.447 ns |  1.00 | 0.0136 |     128 B |        1.00 |
| Struct   | 87.22 ns | 0.461 ns | 0.409 ns |  0.98 | 0.0136 |     128 B |        1.00 |

</div>

### Interpreting Statistics

The above tables include a number of different metrics, which make use of the following terms:

* _Mean:_ Arithmetic mean of all measurements
* _Error:_ Half of 99.9% confidence interval
* _StdDev:_ Standard deviation of all measurements
* _Ratio:_ Mean of the ratio distribution ([Current]/[Baseline])
* _RatioSD:_ Standard deviation of the ratio distribution ([Current]/[Baseline])
* _Gen0:_ GC Generation 0 collects per 1000 operations
* _Allocated:_ Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
* _Alloc Ratio:_ Allocated memory ratio distribution ([Current]/[Baseline])
* _1 ns:_ 1 Nanosecond (0.000000001 sec)

### System Setup

The benchmarks were run under the following specifications:

* _BenchmarkDotNet:_ v0.15.8
* _Operating System:_ Windows 11 (10.0.26200.8037/25H2/2025Update/HudsonValley2)
* _Computer:_ 13th Gen Intel Core i7-1360P 2.20GHz, 1 CPU, 16 logical and 12 physical cores
* _.NET SDK:_ 10.0.104
* _GC Strategy:_ Concurrent Workstation
* _Run Strategy:_ Throughput
* _Hardware Intrinsics:_
  * AVX2+BMI1+BMI2+F16C+FMA+LZCNT+MOVBE
  * AVX
  * SSE3+SSSE3+SSE4.1+SSE4.2+POPCNT
  * X86Base+SSE+SSE2
  * AES+PCLMUL
  * AvxVnni
  * SERIALIZE VectorSize=256
* _[Host]:_ .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 RELEASE
* _Job-CEIKLR:_ .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3

### Further Reading

+ [Project: BenchmarkDotNet][1]
+ [Project: NanoidNet][2]
+ [How-To: Create a Default NanoId][3]
+ [How-To: Work with NanoId strings][4]

### Copyright

The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: https://benchmarkdotnet.org/
[2]: https://github.com/codeyu/nanoid-net
[3]: ../guides/nanoiddefault.html
[4]: ../guides/nanoidstring.html
