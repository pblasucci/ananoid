namespace MulberryLabs.Ananoid.Perf

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Engines
open Nanoid
open MulberryLabs.Ananoid
open MulberryLabs.Ananoid.Core
open LetterSets


[<SimpleJob(RunStrategy.Throughput)>]
type AnanoidVsNanoidNet() =
  [<Benchmark>]
  member me.NanoidNet() = Nanoid.Generate()

  [<Benchmark(Baseline = true)>]
  member me.Ananoid() = nanoId ()


[<SimpleJob(RunStrategy.Throughput)>]
type AllAlphabets() =
  [<Benchmark>]
  member me.Alphanumeric() = nanoIdOf Alphanumeric 21

  [<Benchmark>]
  member me.HexadecimalLowercase() = nanoIdOf HexadecimalLowercase 21

  [<Benchmark>]
  member me.HexadecimalUppercase() = nanoIdOf HexadecimalUppercase 21

  [<Benchmark>]
  member me.Lowercase() = nanoIdOf Lowercase 21

  [<Benchmark>]
  member me.NoLookalikes() = nanoIdOf NoLookalikes 21

  [<Benchmark>]
  member me.NoLookalikesSafe() = nanoIdOf NoLookalikesSafe 21

  [<Benchmark>]
  member me.Numbers() = nanoIdOf Numbers 21

  [<Benchmark>]
  member me.Uppercase() = nanoIdOf Uppercase 21

  [<Benchmark(Baseline = true)>]
  member me.UrlSafe() = nanoIdOf UrlSafe 21


[<SimpleJob(RunStrategy.Throughput)>]
type FunctionVsStruct() =
  [<Benchmark(Baseline = true)>]
  member me.Function() = nanoId ()

  [<Benchmark>]
  member me.Struct() = NanoId.NewId()


module Program =

  open BenchmarkDotNet.Running

  let benchmarks =
    [|
      typeof<AnanoidVsNanoidNet>
      typeof<FunctionVsStruct>
      typeof<AllAlphabets>
    |]

  [<EntryPoint>]
  let main args =
    BenchmarkSwitcher.FromTypes(benchmarks).Run(args) |> ignore
    0 // SUCCESS!
