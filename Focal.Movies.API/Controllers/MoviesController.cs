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
        return Ok();
    }
}