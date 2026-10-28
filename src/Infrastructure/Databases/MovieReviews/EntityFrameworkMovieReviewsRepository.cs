namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews;

using System;
using Application.Authors;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Movies;
using Application.Reviews;
using Extensions;
using Mapping;
using Microsoft.EntityFrameworkCore;
using Models;
using ApplicationAuthor = Application.Authors.Entities.Author;
using ApplicationMovie = Application.Movies.Entities.Movie;
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

        _ = this.context.Database.EnsureDeleted();
        _ = this.context.Database.EnsureCreated();
        _ = this.context.AddData();
    }

    #region Authors

    public virtual async Task<List<ApplicationAuthor>> GetAuthors(CancellationToken cancellationToken)
    {
        var authors = await this.context.Authors
            .Include(a => a.Reviews)
            .ThenInclude(r => r.ReviewedMovie)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return [.. authors.Select(this.mapper.ToDomain)];
    }

    public virtual async Task<ApplicationAuthor> GetAuthorById(Guid id, CancellationToken cancellationToken)
    {
        var author = await this.context.Authors
            .Where(r => r.Id == id).Include(a => a.Reviews)
            .ThenInclude(r => r.ReviewedMovie)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.ToDomain(author);
    }

    public virtual async Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    public virtual async Task<(List<ApplicationAuthor> Items, int TotalCount)> SearchAuthors(
        string query,
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var authors = this.context.Authors.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();

            authors = authors.Where(a => (a.FirstName + " " + a.LastName).Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = await authors.CountAsync(cancellationToken);

        var result = await authors
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ThenBy(a => a.Id)
            .Skip(offset)
            .Take(limit)
            .Include(a => a.Reviews)
            .ThenInclude(r => r.ReviewedMovie)
            .ToListAsync(cancellationToken);

        return ([.. result.Select(this.mapper.ToDomain)], totalCount);
    }

    #endregion Authors

    #region Movies

    public virtual async Task<List<ApplicationMovie>> GetMovies(CancellationToken cancellationToken)
    {
        var result = await this.context.Movies
            .Include(m => m.Reviews)
            .ThenInclude(r => r.ReviewAuthor)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return [.. result.Select(this.mapper.ToDomain)];
    }

    public virtual async Task<ApplicationMovie> GetMovieById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Movies
            .Where(r => r.Id == id)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.ReviewAuthor)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.ToDomain(result);
    }

    public virtual async Task<bool> MovieExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Movies.AsNoTracking().AnyAsync(m => m.Id == id, cancellationToken);
    }

    public virtual async Task<(List<ApplicationMovie> Items, int TotalCount)> SearchMovies(
        string query,
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var movies = this.context.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();

            movies = movies.Where(m => m.Title.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = await movies.CountAsync(cancellationToken);

        var result = await movies
            .OrderBy(m => m.Title)
            .ThenBy(m => m.Id)
            .Skip(offset)
            .Take(limit)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.ReviewAuthor)
            .ToListAsync(cancellationToken);

        return ([.. result.Select(this.mapper.ToDomain)], totalCount);
    }

    #endregion Movies

    #region Reviews

    public async Task<ApplicationReview> CreateReview(
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken)
    {
        var review = new Review
        {
            ReviewAuthorId = authorId,
            ReviewedMovieId = movieId,
            Stars = stars,
            DateCreated = this.timeProvider.GetUtcNow().UtcDateTime,
            DateModified = this.timeProvider.GetUtcNow().UtcDateTime
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
        try
        {
            _ = this.context.Remove(this.context.Reviews.Single(r => r.Id == id));
            _ = await this.context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<List<ApplicationReview>> GetReviews(CancellationToken cancellationToken)
    {
        var result = await this.context.Reviews
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return [.. result.Select(this.mapper.ToDomain)];
    }

    public async Task<ApplicationReview> GetReviewById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Reviews
            .Where(r => r.Id == id)
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.ToDomain(result);
    }

    public async Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<(List<ApplicationReview> Items, int TotalCount)> SearchReviews(
        Guid? movieId,
        Guid? authorId,
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var reviews = this.context.Reviews.AsNoTracking();

        if (movieId.HasValue)
        {
            reviews = reviews.Where(r => r.ReviewedMovieId == movieId.Value);
        }

        if (authorId.HasValue)
        {
            reviews = reviews.Where(r => r.ReviewAuthorId == authorId.Value);
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

    public async Task<bool> UpdateReview(
        Guid id,
        Guid authorId,
        Guid movieId,
        int stars,
        CancellationToken cancellationToken)
    {
        try
        {
            var review = this.context.Reviews.FirstOrDefault(r => r.Id == id);

            NotFoundException.ThrowIfNull(review, EntityType.Review);

            review.Stars = stars;
            review.ReviewAuthorId = authorId;
            review.ReviewedMovieId = movieId;
            review.DateModified = this.timeProvider.GetUtcNow().UtcDateTime;

            _ = this.context.Update(review);
            _ = await this.context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    #endregion Reviews
}
