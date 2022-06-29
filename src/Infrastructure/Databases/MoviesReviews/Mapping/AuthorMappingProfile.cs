namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Mapping;

using AutoMapper;
using Application = Application.Authors.Entities;
using Infrastructure = Models;

public class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile()
    {
        _ = this.CreateMap<Infrastructure.Author, Application.Author>().ReverseMap();
    }
}
