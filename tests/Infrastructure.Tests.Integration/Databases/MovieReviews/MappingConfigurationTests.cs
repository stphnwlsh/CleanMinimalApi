namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using AutoMapper;
using Xunit;

[Collection("MovieReviews")]
public class MappingConfigurationTests
{
    private readonly IMapper mapper;

    public MappingConfigurationTests(MovieReviewsDataFixture fixture)
    {
        this.mapper = fixture.Mapper;
    }

    [Fact]
    public void ShouldHaveValidMappingConfiguration()
    {
        this.mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
