(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid.Perf

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Engines
open BenchmarkDotNet.Running
open Nanoid
open pblasucci.Ananoid
open pblasucci.Ananoid.Core
open LetterSets


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type AnanoidVsNanoidNet() =
  [<Benchmark(Baseline = true)>]
  member me.NanoidNet() = Nanoid.Generate()

  [<Benchmark>]
  member me.Ananoid() = nanoId ()


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type AllAlphabets() =
  let (TargetSize size) = NanoIdOptions.UrlSafe

  [<Benchmark>]
  member me.Alphanumeric() = nanoIdOf Alphanumeric size

  [<Benchmark>]
  member me.HexadecimalLowercase() = nanoIdOf HexadecimalLowercase size

  [<Benchmark>]
  member me.HexadecimalUppercase() = nanoIdOf HexadecimalUppercase size

  [<Benchmark>]
  member me.Lowercase() = nanoIdOf Lowercase size

  [<Benchmark>]
  member me.NoLookalikes() = nanoIdOf NoLookalikes size

  [<Benchmark>]
  member me.NoLookalikesSafe() = nanoIdOf NoLookalikesSafe size

  [<Benchmark>]
  member me.Numbers() = nanoIdOf Numbers size

  [<Benchmark>]
  member me.Uppercase() = nanoIdOf Uppercase size

  [<Benchmark(Baseline = true)>]
  member me.UrlSafe() = nanoIdOf UrlSafe size


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type FunctionVsStruct() =
  [<Benchmark(Baseline = true)>]
  member me.Function() = nanoId ()

  [<Benchmark>]
  member me.Struct() = NanoId.NewId()


module Program =
  let benchmarks = [|
    typeof<AnanoidVsNanoidNet>
    typeof<FunctionVsStruct>
    typeof<AllAlphabets>
  |]

  [<EntryPoint>]
  let main args =
    BenchmarkSwitcher.FromTypes(benchmarks).Run(args) |> ignore
    0 // SUCCESS!
