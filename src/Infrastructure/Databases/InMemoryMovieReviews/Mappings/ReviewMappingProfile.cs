namespace CleanMinimalApi.Infrastructure.Databases.InMemoryMovieReviews.Mappings;

using AutoMapper;
using Application = Application.Entities;
using Infrastructure = Models;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile() => _ = this.CreateMap<Application.Review, Infrastructure.Review>().ReverseMap();
}
