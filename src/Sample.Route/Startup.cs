using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Route.SwaggerHelper;

namespace Sample.Route
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddApiVersioning(config =>
                {
                    config.DefaultApiVersion = new ApiVersion(2, 0);
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    config.ReportApiVersions = true;
                })
                .AddSwaggerConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
                app.UseSwaggerUI(options =>
                {
                    options.DocumentTitle = "Sample api with Route";
                    options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "[DEPRECATED] Sample Route Versioning - v1.0");
                    options.SwaggerEndpoint("/swagger/v2.0/swagger.json", "Sample Route Versioning - v2.0");
                });
            }

            if (!env.IsEnvironment("Test"))
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
