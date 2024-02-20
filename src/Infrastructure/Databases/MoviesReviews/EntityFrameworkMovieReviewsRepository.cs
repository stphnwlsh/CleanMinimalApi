namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews;

using System;
using Application.Authors;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Movies;
using Application.Reviews;
using AutoMapper;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Models;
using ApplicationAuthor = Application.Authors.Entities.Author;
using ApplicationMovie = Application.Movies.Entities.Movie;
using ApplicationReview = Application.Reviews.Entities.Review;

internal class EntityFrameworkMovieReviewsRepository : IAuthorsRepository, IMoviesRepository, IReviewsRepository
{
    private readonly MovieReviewsDbContext context;
    private readonly TimeProvider timeProvider;
    private readonly IMapper mapper;

    public EntityFrameworkMovieReviewsRepository(
        MovieReviewsDbContext context,
        TimeProvider timeProvider,
        IMapper mapper)
    {
        this.context = context;
        this.timeProvider = timeProvider;
        this.mapper = mapper;

        if (this.context != null)
        {
            _ = this.context.Database.EnsureDeleted();
            _ = this.context.Database.EnsureCreated();
            _ = this.context.AddData();
        }
    }

    #region Authors

    public virtual async Task<List<ApplicationAuthor>> GetAuthors(CancellationToken cancellationToken)
    {
        var authors = await this.context.Authors
            .Include(a => a.Reviews)
            .ThenInclude(r => r.ReviewedMovie)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return this.mapper.Map<List<ApplicationAuthor>>(authors);
    }

    public virtual async Task<ApplicationAuthor> GetAuthorById(Guid id, CancellationToken cancellationToken)
    {
        var author = await this.context.Authors
            .Where(r => r.Id == id).Include(a => a.Reviews)
            .ThenInclude(r => r.ReviewedMovie)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.Map<ApplicationAuthor>(author);
        ;
    }

    public virtual async Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
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

        return this.mapper.Map<List<ApplicationMovie>>(result);
    }

    public virtual async Task<ApplicationMovie> GetMovieById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Movies
            .Where(r => r.Id == id)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.ReviewAuthor)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.Map<ApplicationMovie>(result);
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

        return this.mapper.Map<ApplicationReview>(result);
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

        return this.mapper.Map<List<ApplicationReview>>(result);
    }

    public async Task<ApplicationReview> GetReviewById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Reviews
            .Where(r => r.Id == id)
            .Include(r => r.ReviewAuthor)
            .Include(r => r.ReviewedMovie)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.Map<ApplicationReview>(result);
    }

    public async Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);
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
