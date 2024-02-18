namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Mapping;

using AutoMapper;
using Application = Application.Authors.Entities;
using Infrastructure = Models;

internal class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile()
    {
        _ = this.CreateMap<Application.Author, Infrastructure.Author>()
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();

        _ = this.CreateMap<Application.ReviewAuthor, Infrastructure.Author>()
            .ForMember(d => d.Reviews, o => o.Ignore())
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();
    }
}
