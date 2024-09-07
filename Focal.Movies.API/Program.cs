using Focal.Movies.API.AutoMapperProfiles;
using Focal.Movies.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiExplorerService(builder.Configuration)
    .AddCsvService(builder.Configuration)
    .AddAutoMapper(config =>
    {
        config.AddProfile<CsvMapperProfile>();
    })
    .AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

app.Run();