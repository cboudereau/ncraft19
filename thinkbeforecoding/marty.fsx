// this is a time series
// d is for date
// series will be usefull to encode the presence of marty in time !
type Series<'d,'t> = 
  Series of initialValue: 't * gauges: ('d*'t) list

module Series =
  // returns a Series whose value is x on all time
  let always x = Series(x,[])

  // applies the function f to each value in the Series
  let map f (Series(z,xs)) = 
      Series(f z, [ for d,x in xs -> d, f x])

  // normalizes a Series by merging adjacent gauges
  // that have the same value
  let normalize (Series(z,xs)) =
      let rec loop z xs  =
          [ match xs with
            | [] -> ()
            | (_,v) :: r when v = z ->
                yield! loop z r
            | (d,v) :: r ->
              yield d,v
              yield! loop v r ]

      Series(z, loop z xs)

  // apply the function f to a pair of times series
  // by zipping them along time
  let map2 f (Series(xz, xs)) (Series(yz, ys)) =
      let rec loop xz xs yz ys =
          [ match xs, ys with
            | [] , [] -> ()
            | (xd, xv) :: xr, [] ->
              yield xd, f xv yz
              yield! loop xv xr yz ys 
            | [], (yd, yv) :: yr ->
              yield yd, f xz yv
              yield! loop xz xs yv yr
            | (xd, xv) :: xr, (yd, yv) :: yr ->
              if xd < yd then
                  yield xd, f xv yz
                  yield! loop xv xr yz ys 
              elif yd < xd then
                  yield yd, f xz yv
                  yield! loop xz xs yv yr 
              else
                  yield xd, f xv yv
                  yield! loop xv xr yv yr ]

      Series(f xz yz, loop xz xs yz ys)

  // applicative operation, 
  // enables to give series parameter to non-series functions !
  let apply fs xs = map2 (fun f x -> f x) fs xs

  // creates a series from start => end, value
  // on start, we have Some value
  // after end, we have None 
  // convenient way to build our timelines
  let ofTimeline values =
    let rec build values =
        [ match values with
          | ((start, end'), value) :: (((nextStart,_), _) :: _  as rest) -> 
            yield start, Some value
            if end' <> nextStart then
              yield end', None
              yield! build rest 
          | [(start, end'), value] ->
              yield start, Some value
              yield end', None
          | [] -> ()
          ]
    Series(None, build values)



/// these are the operators to apply series values to non-series functions
let (<!>) = Series.map
let (<*>) = Series.apply

// buids a start / end pair
let (=>) start end' = start,end'


// Marty is either from 85 or 2015
type Marty = 
  | Marty85
  | Marty2015

// We have 4 timelines
type Timeline = T1 | T2 | T2' | T3

// this mark the timeline on all points of the serries
let timeline timeline series =
  series
  |> Series.map (Option.map (fun marty ->  marty, timeline))

// more readable notation for dates
let date d m y h min = System.DateTime(y,m,d,h,min,0)
let oct d = date d 10
let nov d = date d 11

// this is the timeline from the first movie
let timeline1 =
    Series.ofTimeline
        [ oct 21 1985  10 05 => oct 21 1985  14 05, Marty85 ]
    |> timeline T1

// this is the timeline from the second movie
let timeline2 =
    Series.ofTimeline
        [ nov 12 1955  13 50 => nov 12 1955  23 30, Marty85
          oct 21 1985  15 07 => oct 21 1985  16 00, Marty85 
          oct 21 2015  15 00 => oct 21 2015  18 00, Marty85 ]
    |> timeline T2

// but there is actually another timeline in this second movie 
// when Marty from 2015 comes back
let timeline2' = 
    Series.ofTimeline
            [ date 21 10 2015 15 0 => date 22 10 2015 0 0, Marty2015 ]
    |> timeline T2'

// this is the timeline from the 3rd movie
let timeline3 = 
    Series.ofTimeline
            [ nov 12 1955  17 00 => nov 12 1955  23 30, Marty85
              oct 21 1885  10 00 => oct 22 1885  23 00, Marty85
              oct 22 2015  15 05 => oct 22 2015  20 00, Marty85 ]
    |> timeline T3

// this function check at a point in time the presence or not of multiple Marty.
let  ``does Marty McFly meet himself`` t1 t2 t2' t3 =
    let presentMartys = List.choose id [ t1; t2; t2'; t3]
    match presentMartys with
    | first :: second :: _ -> Some presentMartys
    | _ -> None 

// this is all for the pretty printing
let sprintMarty =
  function
  | Marty85, tl -> sprintf "from 85 (%A)" tl
  | Marty2015, tl -> sprintf "from 2015 (%A)" tl

let sprintMartys martys =
  martys
  |>List.map sprintMarty
  |> String.concat " and "

let printPointInTime =
  function
  | date, Some martys ->
      printf "Marties %s can meet from %A " (sprintMartys martys) date
  | date, None ->
      printfn "to %A" date

let printResult (Series(_, xs)) =
  xs |> List.iter printPointInTime


// now we can apply the function on the timeline series
// to get the final result !
``does Marty McFly meet himself``
<!> timeline1
<*> timeline2
<*> timeline2'
<*> timeline3
|> Series.normalize
|> printResult



