namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Mapping;

using AutoMapper;
using Application = Application.Reviews.Entities;
using Infrastructure = Models;

internal class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        _ = this.CreateMap<Application.Review, Infrastructure.Review>()
            .ForMember(d => d.ReviewAuthorId, o => o.Ignore())
            .ForMember(d => d.ReviewAuthor, o => o.MapFrom(s => s.ReviewAuthor))
            .ForMember(d => d.ReviewedMovieId, o => o.Ignore())
            .ForMember(d => d.ReviewedMovie, o => o.MapFrom(s => s.ReviewedMovie))
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();
    }
}
