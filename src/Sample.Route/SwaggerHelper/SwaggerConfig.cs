using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Sample.Route.SwaggerHelper
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
            => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Sample Route Versioning - v1.0", Version = "v1.0" });
                c.SwaggerDoc("v2.0", new OpenApiInfo { Title = "Sample Route Versioning - v2.0", Version = "v2.0" });
                c.OperationFilter<SwaggerParameterFilters>();
                c.DocumentFilter<SwaggerVersionMapping>();

                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out var methodInfo))
                    {
                        return false;
                    }

                    var versions = methodInfo.DeclaringType?
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
                    version = version.Replace("v", "");
                    return versions?.Any(v => v.ToString() == version && maps.Any(v => v.ToString() == version)) == true;
                });
            });

        public enum VersioningType
        {
            None, CustomHeader, QueryString, AcceptHeader
        }

        public static string QueryStringParam { get; private set; } = "";
        public static string CustomHeaderParam { get; private set; } = "";
        public static string AcceptHeaderParam { get; private set; } = "";

        public static VersioningType CurrentVersioningMethod = VersioningType.None;

        public static void UseCustomHeaderApiVersion(string parameterName)
        {
            CurrentVersioningMethod = VersioningType.CustomHeader;
            CustomHeaderParam = parameterName;
        }

        public static void UseQueryStringApiVersion()
        {
            QueryStringParam = "api-version";
            CurrentVersioningMethod = VersioningType.QueryString;
        }

        public static void UseQueryStringApiVersion(string parameterName)
        {
            CurrentVersioningMethod = VersioningType.QueryString;
            QueryStringParam = parameterName;
        }

        public static void UseAcceptHeaderApiVersion(String paramName)
        {
            CurrentVersioningMethod = VersioningType.AcceptHeader;
            AcceptHeaderParam = paramName;
        }
    }
}
