using Asp.Versioning;
using AutoMapper;
using Focal.Movies.API.Dtos.Requests;
using Focal.Movies.API.Dtos.Responses;
using Focal.Movies.API.Models;
using Focal.Movies.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Focal.Movies.API.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class MoviesController(MetadataService metadataService, StatsService statsService, IMapper mapper) : ControllerBase
{
    [HttpPost("metadata")]
    public async Task<IActionResult> AddMetadata([FromBody] AddMovieMetadata model, CancellationToken cancellationToken)
    {
        var metadata = mapper.Map<Metadata>(model);
        metadataService.Add(metadata);
        return Ok();
    }

    [HttpGet("metadata/{movieId}")]
    public IActionResult GetMetadata([FromRoute] int movieId)
    {
        var movies = metadataService.GetAll(
                    predicate: m => m.MovieId == movieId,
                    sortExpression: m => m.Id,
                    ascending: false)
                .DistinctBy(m => m.Language)
                .ToList();
        
        if (!movies.Any())
            return NotFound();
        
        return Ok(mapper.Map<IEnumerable<MovieMetadata>>(movies));
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = statsService.GetAll<object>();
        var moviesStatsAvrage = stats
            .GroupBy(s => s.MovieId)
            .Select(g => new
            {
                MovieId = g.Key,
                AverageWatchDurationS = g.Sum(w => w.WatchDurationMs) / g.Count(),
                Watches = g.Count()
            })
            .OrderByDescending(ms => ms.Watches);
        var moviesStats = new List<MovieStats>();
        foreach (var movieStatAverage in moviesStatsAvrage)
        {
            var movieId = movieStatAverage.MovieId;
            var defaultLanguage = "EN";
            var movieMetadata = metadataService.FirstOrDefault<object>(m => m.MovieId == movieId && m.Language == defaultLanguage);

            //Added Cause movieId 8 in stats but not found in metadata
            if (movieMetadata != null)
            {
                moviesStats.Add(new MovieStats
                {
                    MovieId = movieStatAverage.MovieId,
                    ReleaseYear = movieMetadata.ReleaseYear,
                    Title = movieMetadata.Title,
                    AverageWatchDurationS = movieStatAverage.AverageWatchDurationS,
                    Watches = movieStatAverage.Watches
                });
            }
        }
        return Ok(moviesStats);
    }
}