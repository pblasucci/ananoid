(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid.Perf

open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Engines
open BenchmarkDotNet.Running
open NanoidDotNet
open pblasucci.Ananoid
open pblasucci.Ananoid.Core
open LetterSets


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type AnanoidVsNanoidNetVsGuid() =
  [<Benchmark(Baseline = true)>]
  member _.Guid() = Guid.NewGuid()

  [<Benchmark>]
  member _.NanoidNet() = Nanoid.Generate()

  [<Benchmark>]
  member _.Ananoid() = nanoId ()


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type AllAlphabets() =
  [<Benchmark>]
  member me.Alphanumeric() = nanoIdOf Alphanumeric Defaults.Size

  [<Benchmark>]
  member me.HexadecimalLowercase() = nanoIdOf HexadecimalLowercase Defaults.Size

  [<Benchmark>]
  member me.HexadecimalUppercase() = nanoIdOf HexadecimalUppercase Defaults.Size

  [<Benchmark>]
  member me.Lowercase() = nanoIdOf Lowercase Defaults.Size

  [<Benchmark>]
  member me.NoLookalikes() = nanoIdOf NoLookalikes Defaults.Size

  [<Benchmark>]
  member me.NoLookalikesSafe() = nanoIdOf NoLookalikesSafe Defaults.Size

  [<Benchmark>]
  member me.Numbers() = nanoIdOf Numbers Defaults.Size

  [<Benchmark>]
  member me.Uppercase() = nanoIdOf Uppercase Defaults.Size

  [<Benchmark(Baseline = true)>]
  member me.UrlSafe() = nanoIdOf UrlSafe Defaults.Size


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type FunctionVsStruct() =
  [<Benchmark(Baseline = true)>]
  member me.Function() = nanoId ()

  [<Benchmark>]
  member me.Struct() = NanoId.NewId()


module Program =
  let benchmarks = [|
    typeof<AnanoidVsNanoidNetVsGuid>
    typeof<FunctionVsStruct>
    typeof<AllAlphabets>
  |]

  [<EntryPoint>]
  let main args =
    BenchmarkSwitcher.FromTypes(benchmarks).Run(args) |> ignore
    0 // SUCCESS!
