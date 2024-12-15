// For more information see https://aka.ms/fsharp-console-apps
//printfn "Hello from F#"

open System
open System.Windows.Forms
open System.IO
open System.Text.Json

// Define a type for dictionary entries
type DictionaryEntry = {
    Word: string
    Definition: string
}

// Create a mutable dictionary (F# Map)
let mutable dictionary = Map.empty<string, string>

