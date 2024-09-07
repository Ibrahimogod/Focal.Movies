using Focal.Movies.API.Options;
using Focal.Movies.API.Services;

namespace Focal.Movies.API.Extensions;

public static class CsvServiceExtensions
{
     public static IServiceCollection AddCsvService(this IServiceCollection services, IConfiguration configuration)
     {
          return services
               .Configure<CsvFileOptions>(configuration.GetSection(nameof(CsvFileOptions)))
               .AddSingleton<MetadataService>()
               .AddSingleton<StatsService>()
               .AddHostedService<MetadataService>()
               .AddHostedService<StatsService>();
     }
}