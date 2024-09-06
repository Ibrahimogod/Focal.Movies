using Asp.Versioning;
using CsvHelper;
using Focal.Movies.API.AutoMapperProfiles;
using Focal.Movies.API.Options;
using Focal.Movies.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CsvFileOptions>(builder.Configuration.GetSection(nameof(CsvFileOptions)));

builder
    .Services
    .AddSingleton<MetadataService>()
    .AddSingleton<StatsService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CsvMapperProfile>();
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(builder.Configuration.GetValue<int>("DefaultApiVersion"));
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

app.Run();