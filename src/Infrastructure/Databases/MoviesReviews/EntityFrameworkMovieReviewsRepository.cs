namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews;

using Application.Authors;
using Application.Authors.Entities;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Movies;
using Application.Movies.Entities;
using Application.Reviews;
using Application.Reviews.Entities;
using Extensions;
using Microsoft.EntityFrameworkCore;
using SimpleDateTimeProvider;

internal class EntityFrameworkMovieReviewsRepository : IAuthorsRepository, IMoviesRepository, IReviewsRepository
{
    private readonly MovieReviewsDbContext context;
    private readonly IDateTimeProvider dateTimeProvider;

    public EntityFrameworkMovieReviewsRepository(MovieReviewsDbContext context, IDateTimeProvider dateTimeProvider)
    {
        this.context = context;
        this.dateTimeProvider = dateTimeProvider;

        if (this.context != null)
        {
            _ = this.context.Database.EnsureDeleted();
            _ = this.context.Database.EnsureCreated();
            _ = this.context.AddData();
        }
    }

    #region Authors

    public virtual async Task<List<Author>> GetAuthors(CancellationToken cancellationToken)
    {
        return await this.context.Authors.Include(a => a.Reviews).ThenInclude(r => r.ReviewedMovie).AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<Author> GetAuthorById(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.Where(r => r.Id == id).Include(a => a.Reviews).ThenInclude(r => r.ReviewedMovie).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    #endregion Authors

    #region Movies

    public virtual async Task<List<Movie>> GetMovies(CancellationToken cancellationToken)
    {
        return await this.context.Movies.Include(m => m.Reviews).ThenInclude(r => r.ReviewAuthor).AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<Movie> GetMovieById(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Movies.Where(r => r.Id == id).Include(m => m.Reviews).ThenInclude(r => r.ReviewAuthor).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<bool> MovieExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Movies.AsNoTracking().AnyAsync(m => m.Id == id, cancellationToken);
    }

    #endregion Movies

    #region Reviews

    public async Task<Review> CreateReview(Guid authorId, Guid movieId, int stars, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            ReviewAuthorId = authorId,
            ReviewedMovieId = movieId,
            Stars = stars,
            DateCreated = this.dateTimeProvider.UtcNow,
            DateModified = this.dateTimeProvider.UtcNow
        };

        var id = this.context.Add(review).Entity.Id;

        _ = await this.context.SaveChangesAsync(cancellationToken);

        return await this.context.Reviews.Where(r => r.Id == id).Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().FirstAsync(cancellationToken);
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

    public async Task<List<Review>> GetReviews(CancellationToken cancellationToken)
    {
        return await this.context.Reviews.Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Review> GetReviewById(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Reviews.Where(r => r.Id == id).Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> UpdateReview(Guid id, Guid authorId, Guid movieId, int stars, CancellationToken cancellationToken)
    {
        try
        {
            var review = this.context.Reviews.FirstOrDefault(r => r.Id == id);

            NotFoundException.ThrowIfNull(review, EntityType.Review);

            review.Stars = stars;
            review.ReviewAuthorId = authorId;
            review.ReviewedMovieId = movieId;
            review.DateModified = this.dateTimeProvider.UtcNow;

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
