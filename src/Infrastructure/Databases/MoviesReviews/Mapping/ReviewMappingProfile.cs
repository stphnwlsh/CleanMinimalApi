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
            .ForMember(d => d.ReviewedMovieId, o => o.Ignore())
            .ReverseMap();
    }
}
