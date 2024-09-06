namespace Focal.Movies.API.Dtos.Responses;

public record MovieMetadata
{
    public int MovieId { get; set; }
    public  string Title { get; set; }
    public string Language { get; set; }
    public string Duration { get; set; }
    public int ReleaseYear { get; set; }
}