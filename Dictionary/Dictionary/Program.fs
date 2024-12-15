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

// Function to save dictionary to a JSON file
let saveDictionaryToFile (filePath: string) =
    let json = JsonSerializer.Serialize(dictionary)
    File.WriteAllText(filePath, json)

// Function to load dictionary from a JSON file
let loadDictionaryFromFile (filePath: string) =
    if File.Exists(filePath) then
        let json = File.ReadAllText(filePath)
        dictionary <- JsonSerializer.Deserialize<Map<string, string>>(json)

// Initialize the dictionary by loading data from a file
let dictionaryFilePath = "dictionary.json"
loadDictionaryFromFile(dictionaryFilePath)

// Create the GUI
[<STAThread>]
[<EntryPoint>]
let main argv =
    let form = new Form(Text = "F# Digital Dictionary", Width = 600, Height = 400)

    // Create controls
    let wordLabel = new Label(Text = "Word:", Dock = DockStyle.Top)
    let wordTextBox = new TextBox(Dock = DockStyle.Top)

    let definitionLabel = new Label(Text = "Definition:", Dock = DockStyle.Top)
    let definitionTextBox = new TextBox(Dock = DockStyle.Top)
