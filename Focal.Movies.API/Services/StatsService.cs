using Focal.Movies.API.Models;
using Focal.Movies.API.Options;
using Microsoft.Extensions.Options;

namespace Focal.Movies.API.Services;

public class StatsService(IWebHostEnvironment environment,IOptions<CsvFileOptions> csvFileOptions)
    : CsvService<Stats>(environment.ContentRootPath + csvFileOptions.Value.StatsFilePath, []);