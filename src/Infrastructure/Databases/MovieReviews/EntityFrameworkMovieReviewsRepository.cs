namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews;

using System;
using Application.Authors;
using Application.Movies;
using Application.Reviews;
using Mapping;
using Microsoft.EntityFrameworkCore;
using Models;
using ApplicationAuthor = Application.Authors.Entities.Author;
using ApplicationAuthorSummary = Application.Authors.Entities.AuthorSummary;
using ApplicationMovie = Application.Movies.Entities.Movie;
using ApplicationMovieSummary = Application.Movies.Entities.MovieSummary;
using ApplicationReview = Application.Reviews.Entities.Review;

internal class EntityFrameworkMovieReviewsRepository : IAuthorsRepository, IMoviesRepository, IReviewsRepository
{
    private readonly MovieReviewsDbContext context;
    private readonly TimeProvider timeProvider;
    private readonly MovieReviewsMapper mapper;

    public EntityFrameworkMovieReviewsRepository(
        MovieReviewsDbContext context,
        TimeProvider timeProvider,
        MovieReviewsMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(mapper);

        this.context = context;
        this.timeProvider = timeProvider;
        this.mapper = mapper;
    }

    #region Authors

    public virtual async Task<(List<ApplicationAuthorSummary> Items, int TotalCount)> GetAuthors(
        string? query,
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var authors = this.context.Authors.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = ContainsPattern(query);

            authors = authors.Where(a => EF.Functions.Like(a.FirstName + " " + a.LastName, pattern, EscapeCharacter));
        }

        var totalCount = await authors.CountAsync(cancellationToken);

        // The review count and average are worked out by the database, so the reviews themselves are never loaded.
        var result = await authors
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ThenBy(a => a.Id)
            .Skip(offset)
            .Take(limit)
            .Select(a => new
            {
                a.Id,
                a.FirstName,
                a.LastName,
                ReviewCount = a.Reviews.Count,
                AverageStars = a.Reviews.Average(r => (double?)r.Stars)
            })
            .ToListAsync(cancellationToken);

        return ([.. result.Select(a => new ApplicationAuthorSummary(a.Id, a.FirstName, a.LastName, a.ReviewCount, RoundStars(a.AverageStars)))], totalCount);
    }

    public virtual async Task<ApplicationAuthor?> GetAuthorById(Guid id, CancellationToken cancellationToken)
    {
        var author = await this.context.Authors
            .Where(r => r.Id == id).Include(a => a.Reviews)
            .ThenInclude(r => r.ReviewedMovie)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return author is null ? null : this.mapper.ToDomain(author);
    }

    public virtual async Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    #endregion Authors

    #region Movies

    public virtual async Task<(List<ApplicationMovieSummary> Items, int TotalCount)> GetMovies(
        string? query,
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var movies = this.context.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = ContainsPattern(query);

            movies = movies.Where(m => EF.Functions.Like(m.Title, pattern, EscapeCharacter));
        }

        var totalCount = await movies.CountAsync(cancellationToken);

        // The review count and average are worked out by the database, so the reviews themselves are never loaded.
        var result = await movies
            .OrderBy(m => m.Title)
            .ThenBy(m => m.Id)
            .Skip(offset)
            .Take(limit)
            .Select(m => new
            {
                m.Id,
                m.Title,
                ReviewCount = m.Reviews.Count,
                AverageStars = m.Reviews.Average(r => (double?)r.Stars)
            })
            .ToListAsync(cancellationToken);

        return ([.. result.Select(m => new ApplicationMovieSummary(m.Id, m.Title, m.ReviewCount, RoundStars(m.AverageStars)))], totalCount);
    }

    public virtual async Task<ApplicationMovie?> GetMovieById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Movies
            .Where(r => r.Id == id)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.ReviewAuthor)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return result is null ? null : this.mapper.ToDomain(result);
    }

    public virtual async Task<bool> MovieExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Movies.AsNoTracking().AnyAsync(m => m.Id == id, cancellationToken);
    }

    #endregion Movies

    #region Reviews

    public async Task<ApplicationReview> CreateReview(
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken)
    {
        var now = this.timeProvider.GetUtcNow().UtcDateTime;

        var review = new Review
        {
            ReviewAuthorId = authorId,
            ReviewedMovieId = movieId,
            Stars = stars,
            DateCreated = now,
            DateModified = now
        };

        var id = this.context.Add(review).Entity.Id;

        _ = await this.context.SaveChangesAsync(cancellationToken);

        var result = await this.context.Reviews
            .Where(r => r.Id == id)
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return this.mapper.ToDomain(result);
    }

    public async Task<bool> DeleteReview(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await this.context.Reviews
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return deleted > 0;
    }

    public async Task<(List<ApplicationReview> Items, int TotalCount)> GetReviews(
        Guid? movieId,
        Guid? authorId,
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var reviews = this.context.Reviews.AsNoTracking();

        if (movieId is Guid reviewedMovieId)
        {
            reviews = reviews.Where(r => r.ReviewedMovieId == reviewedMovieId);
        }

        if (authorId is Guid reviewAuthorId)
        {
            reviews = reviews.Where(r => r.ReviewAuthorId == reviewAuthorId);
        }

        var totalCount = await reviews.CountAsync(cancellationToken);

        var result = await reviews
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .OrderBy(r => r.ReviewedMovie.Title)
            .ThenBy(r => r.ReviewAuthor.LastName)
            .ThenBy(r => r.Id)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return ([.. result.Select(this.mapper.ToDomain)], totalCount);
    }

    public async Task<List<ApplicationReview>> GetReviewsForMovies(IReadOnlyCollection<Guid> movieIds, CancellationToken cancellationToken)
    {
        var result = await this.context.Reviews
            .AsNoTracking()
            .Where(r => movieIds.Contains(r.ReviewedMovieId))
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .OrderBy(r => r.ReviewAuthor.LastName)
            .ThenBy(r => r.ReviewAuthor.FirstName)
            .ThenBy(r => r.Id)
            .ToListAsync(cancellationToken);

        return [.. result.Select(this.mapper.ToDomain)];
    }

    public async Task<List<ApplicationReview>> GetReviewsForAuthors(IReadOnlyCollection<Guid> authorIds, CancellationToken cancellationToken)
    {
        var result = await this.context.Reviews
            .AsNoTracking()
            .Where(r => authorIds.Contains(r.ReviewAuthorId))
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .OrderBy(r => r.ReviewedMovie.Title)
            .ThenBy(r => r.Id)
            .ToListAsync(cancellationToken);

        return [.. result.Select(this.mapper.ToDomain)];
    }

    public async Task<ApplicationReview?> GetReviewById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Reviews
            .Where(r => r.Id == id)
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return result is null ? null : this.mapper.ToDomain(result);
    }

    public async Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> UpdateReview(Guid id, int stars, CancellationToken cancellationToken)
    {
        var now = this.timeProvider.GetUtcNow().UtcDateTime;

        var updated = await this.context.Reviews
            .Where(r => r.Id == id)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(r => r.Stars, stars)
                    .SetProperty(r => r.DateModified, now),
                cancellationToken);

        return updated > 0;
    }

    #endregion Reviews

    #region Helpers

    private static double? RoundStars(double? averageStars)
    {
        return averageStars is null ? null : Math.Round(averageStars.Value, 1, MidpointRounding.AwayFromZero);
    }

    #endregion Helpers

    #region Search

    private const string EscapeCharacter = "\\";

    /// <summary>
    /// Builds a LIKE pattern that matches any text containing the search term. LIKE ignores case for
    /// ASCII text in SQLite and SQL Server, and the term's own wildcards are escaped so they match literally.
    /// </summary>
    private static string ContainsPattern(string query)
    {
        var term = query.Trim()
            .Replace(EscapeCharacter, EscapeCharacter + EscapeCharacter, StringComparison.Ordinal)
            .Replace("%", EscapeCharacter + "%", StringComparison.Ordinal)
            .Replace("_", EscapeCharacter + "_", StringComparison.Ordinal);

        return $"%{term}%";
    }

    #endregion Search
}
