using AutoMapper;
using Focal.Movies.API.Dtos.Requests;
using Focal.Movies.API.Dtos.Responses;
using Focal.Movies.API.Models;

namespace Focal.Movies.API.AutoMapperProfiles;

public class CsvMapperProfile : Profile
{
    public CsvMapperProfile()
    {
        CreateMap<AddMovieMetadata, Metadata>();

        CreateMap<Metadata, MovieMetadata>();
    }
}