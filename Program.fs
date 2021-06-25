open System
open System.Reactive.Linq
open Akka.Streams
open Akka.Streams.Dsl
open Akka.Actor

let sourceToObservableNotWorking (materializer : IMaterializer) =
    let data = [1;2;3]
    let streamSource = Source.From data // 1. Create a Source from some data
    let observable = streamSource.RunWith(Sink.AsObservable<_>(), materializer) // 2. Convert that Source to and Observable
    observable
        .Select(fun i -> i.ToString()) // 3. Try to use Select on that Observable
        .Subscribe(fun i -> printfn "--> %A" i) // 4. Subscribe to that Observable then wait until it produces (in this case it doesn't)

[<EntryPoint>]
let main argv =
    printfn "Starting"
    use system = ActorSystem.Create(Guid.NewGuid().ToString("n"))
    use mat = system.Materializer()
    use _d1 = sourceToObservableNotWorking mat

    Console.ReadLine() |> ignore
    printfn "Finishing"
    0 