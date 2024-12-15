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

    let addButton = new Button(Text = "Add/Update", Dock = DockStyle.Top)
    let deleteButton = new Button(Text = "Delete", Dock = DockStyle.Top)
    let searchButton = new Button(Text = "Search", Dock = DockStyle.Top)

    let resultLabel = new Label(Text = "Results:", Dock = DockStyle.Top)
    let resultListBox = new ListBox(Dock = DockStyle.Fill)

    form.Controls.AddRange [| resultListBox; resultLabel; searchButton; deleteButton; addButton; definitionTextBox; definitionLabel; wordTextBox; wordLabel |]

    // Add or update word event
    addButton.Click.Add(fun _ ->
        let word = wordTextBox.Text.Trim()
        let definition = definitionTextBox.Text.Trim()
        if not (String.IsNullOrWhiteSpace(word) || String.IsNullOrWhiteSpace(definition)) then
            dictionary <- dictionary.Add(word.ToLower(), definition)
            MessageBox.Show(sprintf "'%s' added/updated successfully!" word) |> ignore
            saveDictionaryToFile(dictionaryFilePath)
        else
            MessageBox.Show("Word and definition cannot be empty.") |> ignore
    )
    // Delete word event
    deleteButton.Click.Add(fun _ ->
        let word = wordTextBox.Text.Trim().ToLower()
        if dictionary.ContainsKey(word) then
            dictionary <- dictionary.Remove(word)
            MessageBox.Show(sprintf "'%s' deleted successfully!" word) |> ignore
            saveDictionaryToFile(dictionaryFilePath)
        else
            MessageBox.Show(sprintf "'%s' does not exist in the dictionary." word) |> ignore
    )
    // Search word event
    searchButton.Click.Add(fun _ ->
        let keyword = wordTextBox.Text.Trim().ToLower()
        resultListBox.Items.Clear()

        let results =
            dictionary
            |> Map.filter (fun key _ -> key.Contains(keyword))

        if results.Count > 0 then
            for kvp in results do
                resultListBox.Items.Add(sprintf "%s: %s" kvp.Key kvp.Value) |> ignore
        else
            MessageBox.Show("No matching results found.") |> ignore
    )

    Application.Run(form)
    0
