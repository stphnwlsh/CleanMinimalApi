namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Mapping;

using AutoMapper;
using Application = Application.Movies.Entities;
using Infrastructure = Models;

internal class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        _ = this.CreateMap<Application.Movie, Infrastructure.Movie>()
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();

        _ = this.CreateMap<Application.ReviewedMovie, Infrastructure.Movie>()
            .ForMember(d => d.Reviews, o => o.Ignore())
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();
    }
}
