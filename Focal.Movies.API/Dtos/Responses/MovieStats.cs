namespace Focal.Movies.API.Dtos.Responses;

public record MovieStats
{
    public int MovieId { get; set; }
    public string Title { get; set; }
    public long AverageWatchDurationS { get; set; }
    public long Watches { get; set; }
    public int ReleaseYear { get; set; }
}