namespace Sample.Route.Tests

open System.Text.Json

module Serialization =
    let defaultOptions = 
        let options = new JsonSerializerOptions()
        options.PropertyNameCaseInsensitive <- true
        options

    let Deserialize<'T> (text: string) =
        text
        |> fun data -> (data, defaultOptions)
        |> JsonSerializer.Deserialize<'T>


