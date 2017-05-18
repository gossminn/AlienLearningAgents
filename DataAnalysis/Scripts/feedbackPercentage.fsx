open System.IO

let feedback = File.ReadAllLines("Feedback.txt")
let length = feedback |> Array.length

let FindPercentageUntil x = 
    let happyN = 
        feedback 
        |> Array.mapi (fun i e -> i, e)
        |> Array.takeWhile (fun (i, _) -> i < x)
        |> Array.map snd
        |> Array.filter (fun s -> s = "Happy")
        |> Array.length
    ((float happyN) / (float x)) * 100.0
 
{1..length}
|> Seq.map FindPercentageUntil
|> Seq.iter (fun x -> printfn "%f" x)