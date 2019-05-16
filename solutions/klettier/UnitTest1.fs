namespace Tests
module Code = 
    open System
    type Period = {
        Arrival : DateTimeOffset
        Departure : DateTimeOffset
    }
    let private max a b = if a > b then a else b
    let private min a b = if a < b then a else b
    let private getOverlap a b : Period option =
        if (a.Arrival < b.Departure && b.Arrival < a.Departure) then
            Some { Arrival = max a.Arrival b.Arrival ; Departure = min a.Departure b.Departure }
        else
            None
    let periodMeet (timeLine1 : Period list) (timeLine2 : Period list) : Period list =
        let rec p l1 l2 res = 
            match l1 with
            | [] -> res
            | e :: l ->
                List.map (fun d -> getOverlap e d) l2
                |> List.choose id
                |> fun l -> l @ res
                |> fun r -> p l l2 r
        p timeLine1 timeLine2 []
    let periodMeet2 timelines =
        let rec applyOnEach l res = 
            match l with
            | [] -> res
            | e :: l -> l |> List.map (fun t -> (e, t)) |> fun r -> r @ res |> applyOnEach l
        applyOnEach timelines [] |> List.map (fun (t1, t2) -> periodMeet t1 t2) |> List.collect id

module Tests =
    open NUnit.Framework
    open Code
    open System
    type TestClass () =
        let toDate = DateTimeOffset.Parse
        [<Test>]
        member __.``When empty period return empty meet period`` () =
            let result = periodMeet [] []
            Assert.AreEqual([], result) |> ignore
        [<Test>]
        member __.``When single period return empty meet period`` () =
            let period = { Arrival = toDate "1985-10-21T10:05:00Z" ; Departure = toDate "1985-10-21T14:05:00Z" }
            let timeLine = [period]
            let result = periodMeet timeLine []
            let result2 = periodMeet [] timeLine
            Assert.AreEqual([], result) |> ignore
            Assert.AreEqual([], result2) |> ignore
        [<Test>]
        member __.``When two same period return single meet period`` () =
            let period = { Arrival = toDate "1985-10-21T10:05:00Z" ; Departure = toDate "1985-10-21T14:05:00Z" }
            let timeLine = [period]
            let result = periodMeet timeLine timeLine
            Assert.AreEqual(timeLine, result) |> ignore
        [<Test>]
        member __.``When meet then meet`` () =
            let timeLine1 = 
             [
                { Arrival = toDate "1985-10-21T10:05:00Z" ; Departure = toDate "1985-10-21T14:05:00Z" }//1985-10-21T10:05:00Z -> 1985-10-21T14:05:00Z
             ]
            let timeLine2 =
             [
                { Arrival = toDate "1955-11-12T13:50:00Z" ; Departure = toDate "1955-11-12T23:30:00Z" }//1955-11-12T13:50:00Z -> 1955-11-12T23:30:00Z
                { Arrival = toDate "1985-10-21T15:07:00Z" ; Departure = toDate "1985-10-21T16:00:00Z" }//1985-10-21T15:07:00Z -> 1985-10-21T16:00:00Z
                { Arrival = toDate "2015-10-21T15:00:00Z" ; Departure = toDate "2015-10-21T18:00:00Z" }//2015-10-21T15:00:00Z -> 2015-10-21T18:00:00Z
             ]
            let timeLine2Alive = 
             [
                 { Arrival = toDate "2015-01-01T00:00:00Z" ; Departure = toDate "2016-01-01T00:00:00Z"}//2015-01-01T00:00:00Z -> 2016-01-01T00:00:00Z
             ]
            let timeLine3 = 
             [
                { Arrival = toDate "1955-11-12T17:00:00Z" ; Departure = toDate "1955-11-12T23:30:00Z" }//1955-11-12T17:00:00Z -> 1955-11-12T23:30:00Z
                { Arrival = toDate "1885-10-21T10:00:00Z" ; Departure = toDate "1885-10-22T23:00:00Z" }//1885-10-21T10:00:00Z -> 1885-10-22T23:00:00Z
                { Arrival = toDate "2015-10-22T15:05:00Z" ; Departure = toDate "2015-10-22T20:00:00Z" }//2015-10-22T15:05:00Z -> 2015-10-22T20:00:00Z
             ]
            [timeLine1 ; timeLine2 ; timeLine2Alive ; timeLine3]
            |> periodMeet2
            |> fun res -> Assert.AreEqual([
                  {Arrival = toDate "22/10/2015 15:05:00 +00:00" ; Departure = toDate "22/10/2015 20:00:00 +00:00"}
                  {Arrival = toDate "21/10/2015 15:00:00 +00:00" ; Departure = toDate "21/10/2015 18:00:00 +00:00"}
                  {Arrival = toDate "12/11/1955 17:00:00 +00:00" ; Departure = toDate "12/11/1955 23:30:00 +00:00"}
            ], res)
            |> ignore