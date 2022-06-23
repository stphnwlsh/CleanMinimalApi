namespace CleanMinimalApi.Infrastructure.Databases.InMemoryMovieReviews.Mappings;

using AutoMapper;
using Application = Application.Entities;
using Infrastructure = Models;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile() => _ = this.CreateMap<Application.Movie, Infrastructure.Movie>().ReverseMap();
}
