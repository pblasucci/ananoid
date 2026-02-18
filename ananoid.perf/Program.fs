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
open pblasucci.Ananoid.Core.Alphabets


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type AnanoidVsNanoidNetVsGuid() =
  [<Benchmark(Baseline = true)>]
  member _.Guid() = Guid.NewGuid().ToString()

  [<Benchmark>]
  member _.NanoidNet() = Nanoid.Generate()

  [<Benchmark>]
  member _.Ananoid() = nanoId ()


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type AllAlphabets() =
  [<Benchmark>]
  member _.Alphanumeric() = nanoIdOf Alphanumeric Defaults.Size

  [<Benchmark>]
  member _.HexadecimalLowercase() = nanoIdOf HexadecimalLowercase Defaults.Size

  [<Benchmark>]
  member _.HexadecimalUppercase() = nanoIdOf HexadecimalUppercase Defaults.Size

  [<Benchmark>]
  member _.Lowercase() = nanoIdOf Lowercase Defaults.Size

  [<Benchmark>]
  member _.NoLookalikes() = nanoIdOf NoLookalikes Defaults.Size

  [<Benchmark>]
  member _.NoLookalikesSafe() = nanoIdOf NoLookalikesSafe Defaults.Size

  [<Benchmark>]
  member _.Numbers() = nanoIdOf Numbers Defaults.Size

  [<Benchmark>]
  member _.Uppercase() = nanoIdOf Uppercase Defaults.Size

  [<Benchmark(Baseline = true)>]
  member _.UrlSafe() = nanoIdOf UrlSafe Defaults.Size


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type FunctionVsStruct() =
  [<Benchmark(Baseline = true)>]
  member _.Function() = nanoId ()

  [<Benchmark>]
  member _.Struct() = NanoId.NewId()


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type VaryingSizes() =
  [<Params(8, 21, 36, 64, 128, 256)>]
  member val Size = 0 with get, set

  [<Benchmark>]
  member me.Default() = nanoIdOf Defaults.Alphabet me.Size


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type Pow2VsNonPow2() =
  // 64 chars -> power-of-2 -> generatePow2
  [<Benchmark(Baseline = true)>]
  member _.Pow2_UrlSafe() = nanoIdOf UrlSafe Defaults.Size

  // 62 chars -> non-power-of-2 -> generate (with rejection sampling)
  [<Benchmark>]
  member _.NonPow2_Alphanumeric() = nanoIdOf Alphanumeric Defaults.Size

  // 16 chars -> power-of-2
  [<Benchmark>]
  member _.Pow2_Hex() = nanoIdOf HexadecimalLowercase Defaults.Size

  // 10 chars -> non-power-of-2
  [<Benchmark>]
  member _.NonPow2_Numbers() = nanoIdOf Numbers Defaults.Size


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type AlphabetStructVsRawFunction() =
  let alphabet = Alphabet.makeOrRaise UrlSafe

  [<Benchmark(Baseline = true)>]
  member _.RawFunction() = nanoIdOf UrlSafe Defaults.Size

  [<Benchmark>]
  member _.AlphabetStruct() = Alphabet.makeNanoId Defaults.Size alphabet


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type Parsing() =
  let alphabet = Alphabet.makeOrRaise UrlSafe
  let valid = string (alphabet.MakeNanoId Defaults.Size)
  let invalid = String.replicate Defaults.Size "!"

  [<Benchmark>]
  member _.ParseValid() = Alphabet.parseNanoId valid alphabet

  [<Benchmark>]
  member _.ParseInvalid() = Alphabet.parseNanoId invalid alphabet

  [<Benchmark>]
  member _.ParseEmpty() = Alphabet.parseNanoId "" alphabet


[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Throughput)>]
type TaggedVsUntagged() =
  [<Benchmark(Baseline = true)>]
  member _.Untagged() = nanoId ()

  [<Benchmark>]
  member _.Tagged() = Tagged.nanoId' ()


module Program =
  let benchmarks = [|
    typeof<AnanoidVsNanoidNetVsGuid>
    typeof<FunctionVsStruct>
    typeof<AllAlphabets>
    typeof<VaryingSizes>
    typeof<Pow2VsNonPow2>
    typeof<AlphabetStructVsRawFunction>
    typeof<Parsing>
    typeof<TaggedVsUntagged>
  |]

  [<EntryPoint>]
  let main args =
    BenchmarkSwitcher.FromTypes(benchmarks).Run(args) |> ignore
    0 // SUCCESS!
