namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Mapping;

using AutoMapper;
using Application = Application.Movies.Entities;
using Infrastructure = Models;

internal class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        _ = this.CreateMap<Infrastructure.Movie, Application.Movie>()
            .ReverseMap();

        _ = this.CreateMap<Infrastructure.Movie, Application.ReviewMovie>();
    }
}
