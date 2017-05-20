open System.IO

let feedback = File.ReadAllLines("Feedback.txt")
let windowSize = 25

let findPercentageUntil x = 
    let lowIndex = if x > windowSize then x - windowSize else 0
    let lastN = [lowIndex .. x] |> List.map (fun i -> feedback.[i])
    let happy = lastN |> List.filter (fun x -> x = "Happy")
    (float happy.Length) / (float lastN.Length) * 100.0
 
{0 .. feedback.Length - 1}
|> Seq.map findPercentageUntil
|> Seq.iter (fun x -> printfn "%f" x)