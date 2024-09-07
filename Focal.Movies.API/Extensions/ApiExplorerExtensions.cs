using Asp.Versioning;

namespace Focal.Movies.API.Extensions;

public static class ApiExplorerExtensions
{
    public static IServiceCollection AddApiExplorerService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(configuration.GetValue<int>("DefaultApiVersion"));
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader());
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        return services;
    }
}