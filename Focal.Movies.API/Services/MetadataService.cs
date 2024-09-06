using Focal.Movies.API.Models;
using Focal.Movies.API.Options;
using Microsoft.Extensions.Options;

namespace Focal.Movies.API.Services;

public class MetadataService(IWebHostEnvironment environment, IOptions<CsvFileOptions> csvFileOptions)
    : CsvService<Metadata>(environment.ContentRootPath + csvFileOptions.Value.MetadataFilePath, [new ColumnConfiguration
    {
        Name = nameof(Metadata.Id),
        AutoIncrement = true,
        AutoIncrementSeed = 1,
        AutoIncrementStep = 1,
        DataType = typeof(Int32)
    }]);