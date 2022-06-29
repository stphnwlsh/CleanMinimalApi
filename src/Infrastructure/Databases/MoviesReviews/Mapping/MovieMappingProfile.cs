namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Mapping;

using AutoMapper;
using Application = Application.Movies.Entities;
using Infrastructure = Models;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        _ = this.CreateMap<Infrastructure.Movie, Application.Movie>().ReverseMap();
    }
}
