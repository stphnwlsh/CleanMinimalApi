namespace CleanMinimalApi.Infrastructure.Databases.InMemoryMovieReviews.Mappings;

using AutoMapper;
using Application = Application.Entities;
using Infrastructure = Models;

public class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile() => _ = this.CreateMap<Application.Author, Infrastructure.Author>().ReverseMap();
}
