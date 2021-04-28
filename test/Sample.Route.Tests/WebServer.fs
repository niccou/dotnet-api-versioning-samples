namespace Sample.Route.Tests

open System
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection


module WebServer =
    let servicesConfiguration (_: IServiceCollection) = ()
        
    let hostBuilder (builder: IWebHostBuilder) = 
        builder.ConfigureServices(Action<IServiceCollection> servicesConfiguration)
        |> ignore

    let webAppFactory () = 
        new WebApplicationFactory<Sample.Route.Program>()
   
    let createHost() = webAppFactory().WithWebHostBuilder(Action<IWebHostBuilder> hostBuilder)