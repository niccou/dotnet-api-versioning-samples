namespace Sample.Route.Tests

open System.Net.Http

module HttpFunctions =
    let runTask task =
        task
        |> Async.AwaitTask
        |> Async.RunSynchronously

    let get (client: HttpClient) (path: string) =
        path
        |> client.GetAsync
        |> runTask

    let ensureSuccess (response : HttpResponseMessage) = 
        if not response.IsSuccessStatusCode then 
            response.Content.ReadAsStringAsync()
            |> runTask
            |> failwithf "%A"
        else response

    let readText (response : HttpResponseMessage) =
        response.Content.ReadAsStringAsync()
        |> runTask
