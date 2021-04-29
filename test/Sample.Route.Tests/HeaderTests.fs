namespace Sample.Route.Tests

open Xunit
open HttpFunctions
open WebServer

type Header = {
    Key: string
    Value: string seq
}

module HeaderTests =
    [<Literal>]
    let RouteV1 = "api/v1.0/WeatherForecast"

    let callRouteAndGetHeaders route =
        use client = createHost().CreateClient()
        route
        |> get client
        |> ensureSuccess
        |> (fun res -> res.Headers)
        |> Seq.map (fun header -> { Key = header.Key; Value = header.Value })
        |> Seq.cache

    [<Fact>]
    let ``The api return deprecated api version`` () =
        RouteV1
        |> callRouteAndGetHeaders 
        |> Seq.filter (fun h -> h.Key = "api-deprecated-versions")
        |> Seq.length
        |> fun l -> (1, l)
        |> Assert.Equal

    [<Fact>]
    let ``The api return deprecated api version 1_0`` () =
        RouteV1
        |> callRouteAndGetHeaders 
        |> Seq.filter (fun h -> h.Key = "api-deprecated-versions")
        |> Seq.head
        |> fun h -> h.Value |> Seq.head
        |> fun v -> ("1.0", v)
        |> Assert.Equal

    [<Fact>]
    let ``The api return supported api version 2_0`` () =
        let expectedHeader = { Key = "api-supported-versions"; Value = [ "2.0" ] }

        let header = RouteV1
                        |> callRouteAndGetHeaders
                        |> Seq.tryFind (fun h -> h.Key = expectedHeader.Key)

        header
        |> Option.isSome
        |> Assert.True

        header.Value
        |> fun h -> h.Value
        |> Seq.head
        |> fun v -> ("2.0", v)
        |> Assert.Equal
