namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using AutoMapper;
using Xunit;

[Collection("MovieReviews")]
public class MappingConfigurationTests(MovieReviewsDataFixture fixture)
{
    private readonly IMapper mapper = fixture.Mapper;

    [Fact]
    public void ShouldHaveValidMappingConfiguration()
    {
        this.mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
