using CsvHelper.Configuration.Attributes;

namespace Focal.Movies.API.Models;

public class Stats
{
    [Name("movieId")]
    public int MovieId { get; set; }
    [Name("watchDurationMs")]
    public long WatchDurationMs { get; set; }
}