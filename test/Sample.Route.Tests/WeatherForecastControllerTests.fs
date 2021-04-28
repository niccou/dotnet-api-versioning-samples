namespace Sample.Route.Tests


open System
open Xunit
open System.Text.Json
open Sample.Route
open System.Collections.Generic
open HttpFunctions
open WebServer
open FsCheck.Xunit
open FsCheck

module WeatherForecastControllerTests =
    [<Literal>]
    let RouteV1 = "api/v1.0/WeatherForecast"

    [<Literal>]
    let RouteV2 = "api/v2.0/WeatherForecast"
    
    let NewRouteV2 (count: int)= sprintf "api/v2.0/WeatherForecast/%i" count

    let callRouteAndDeserializeTo<'T> route =
        use client = createHost().CreateClient()
        route
        |> get client
        |> ensureSuccess
        |> readText
        |> Serialization.Deserialize<'T>
 

    [<Fact>]
    let ``The api version 1 call should return 5 elements filled with valid date`` () =
        let result =
            RouteV1
            |> callRouteAndDeserializeTo<IEnumerable<WeatherForecast>>
            |> Seq.cache

        result
        |> Assert.NotEmpty

        (5, result |> Seq.length)
        |> Assert.Equal

        result
        |> Seq.map (fun w -> w.Date)
        |> Seq.distinct
        |> Seq.filter (fun d -> d = DateTime.MinValue)
        |> Assert.Empty

    [<Fact>]
    let ``The api version 2 call should return 5 elements filled with valid date`` () =
        let result =
            RouteV2
            |> callRouteAndDeserializeTo<IEnumerable<WeatherForecast>>
            |> Seq.cache

        result
        |> Assert.NotEmpty

        (5, result |> Seq.length)
        |> Assert.Equal

        result
        |> Seq.map (fun w -> w.Date)
        |> Seq.distinct
        |> Seq.filter (fun d -> d = DateTime.MinValue)
        |> Assert.Empty

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(10)>]
    [<InlineData(20)>]
    let ``The new api version with count should return <count> elements`` (count: int) =
        let result =
            count
            |> NewRouteV2
            |> callRouteAndDeserializeTo<IEnumerable<WeatherForecast>>
            |> Seq.cache
            
        (count, result |> Seq.length)
        |> Assert.Equal
