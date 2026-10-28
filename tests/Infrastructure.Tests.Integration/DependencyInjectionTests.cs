namespace CleanMinimalApi.Infrastructure.Tests.Integration;

using Application.Authors;
using Application.Movies;
using Application.Reviews;
using Infrastructure.Databases.MovieReviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_ShouldRegister_ScopedRepositories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        _ = services.AddInfrastructure(InMemoryConnectionString());

        // Assert
        services.Single(s => s.ServiceType == typeof(MovieReviewsDbContext)).Lifetime.ShouldBe(ServiceLifetime.Scoped);
        services.Single(s => s.ServiceType == typeof(IAuthorsRepository)).Lifetime.ShouldBe(ServiceLifetime.Scoped);
        services.Single(s => s.ServiceType == typeof(IMoviesRepository)).Lifetime.ShouldBe(ServiceLifetime.Scoped);
        services.Single(s => s.ServiceType == typeof(IReviewsRepository)).Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddInfrastructure_ShouldUse_DefaultConnectionString()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        _ = services.AddInfrastructure();

        using var provider = services.BuildServiceProvider();

        // Assert
        provider.GetRequiredService<MovieReviewsDatabase>().ConnectionString.ShouldBe(DependencyInjection.DefaultConnectionString);
    }

    [Fact]
    public async Task InitialiseInfrastructure_ShouldCreate_SampleDataOnce()
    {
        // Arrange
        var services = new ServiceCollection();
        _ = services.AddInfrastructure(InMemoryConnectionString());

        using var provider = services.BuildServiceProvider(validateScopes: true);

        // Act
        _ = provider.InitialiseInfrastructure();
        _ = provider.InitialiseInfrastructure();

        // Assert
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MovieReviewsDbContext>();

        (await context.Authors.CountAsync()).ShouldBe(15);
        (await context.Movies.CountAsync()).ShouldBe(50);
        (await context.Reviews.CountAsync()).ShouldBe(150);
    }

    [Fact]
    public async Task InitialiseInfrastructure_ShouldCreate_TheSameSampleData()
    {
        // Arrange
        var first = await SampleTitles();

        // Act
        var second = await SampleTitles();

        // Assert
        second.ShouldBe(first);
    }

    [Fact]
    public async Task Repositories_ShouldHandle_ConcurrentRequests()
    {
        // Arrange
        var services = new ServiceCollection();
        _ = services.AddInfrastructure(InMemoryConnectionString());

        using var provider = services.BuildServiceProvider(validateScopes: true);
        _ = provider.InitialiseInfrastructure();

        // Act
        var requests = Enumerable.Range(0, 20).Select(async _ =>
        {
            using var scope = provider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IMoviesRepository>();

            return await repository.GetMovies(null, 0, 50, CancellationToken.None);
        });

        var results = await Task.WhenAll(requests);

        // Assert
        results.ShouldAllBe(r => r.TotalCount == 50);
    }

    private static async Task<List<string>> SampleTitles()
    {
        var services = new ServiceCollection();
        _ = services.AddInfrastructure(InMemoryConnectionString());

        using var provider = services.BuildServiceProvider();
        _ = provider.InitialiseInfrastructure();

        var (items, _) = await provider.GetRequiredService<IServiceScopeFactory>().CreateScope()
            .ServiceProvider.GetRequiredService<IMoviesRepository>()
            .GetMovies(null, 0, 50, CancellationToken.None);

        return [.. items.Select(m => m.Title)];
    }

    private static string InMemoryConnectionString()
    {
        return $"Data Source=Tests-{Guid.NewGuid()};Mode=Memory;Cache=Shared";
    }
}
