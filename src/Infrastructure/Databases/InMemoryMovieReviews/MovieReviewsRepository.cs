namespace CleanMinimalApi.Infrastructure.Databases.InMemoryMovieReviews;

using AutoMapper;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Movies;
using CleanMinimalApi.Application.Reviews;
using CleanMinimalApi.Infrastructure.Databases.InMemoryMovieReviews.Models;
using Microsoft.EntityFrameworkCore;
using SimpleDateTimeProvider;
using Application = Application.Entities;
using Infrastructure = Models;

internal class MovieReviewsRepository : IAuthorsRepository, IMoviesRepository, IReviewsRepository
{
    private readonly MovieReviewsDbContext context;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IMapper mapper;

    public MovieReviewsRepository(MovieReviewsDbContext context, IDateTimeProvider dateTimeProvider, IMapper mapper)
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

    public async Task<Application.Author> CreateAuthor(string firstName, string lastName, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            FirstName = firstName,
            LastName = lastName,
            DateCreated = this.dateTimeProvider.UtcNow,
            DateModified = this.dateTimeProvider.UtcNow
        };

        var id = this.context.Add(author).Entity.Id;

        _ = await this.context.SaveChangesAsync(cancellationToken);

        var resultAuthor = await this.context.Authors.Where(r => r.Id == id).Include(r => r.Reviews).AsNoTracking().FirstAsync(cancellationToken);

        var mappedAuthor = this.mapper.Map<Application.Author>(resultAuthor);

        return mappedAuthor;
    }

    public virtual async Task<List<Author>> ReadAllAuthors(CancellationToken cancellationToken)
    {
        var resultAuthor = await this.context.Authors.Include(a => a.Reviews).ThenInclude(r => r.ReviewedMovie).AsNoTracking().ToListAsync(cancellationToken);

        var mappedAuthors = this.mapper.Map<Application.Author>(resultAuthor);

        return mappedAuthors
    }

    public virtual async Task<Author> ReadAuthorById(Guid id, CancellationToken cancellationToken) => await this.context.Authors.Where(r => r.Id == id).Include(a => a.Reviews).ThenInclude(r => r.ReviewedMovie).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> UpdateAuthor(Guid id, string firstName, string lastName, CancellationToken cancellationToken)
    {
        try
        {
            var author = this.context.Authors.FirstOrDefault(r => r.Id == id);

            NotFoundException.ThrowIfNull(author, EntityType.Author);

            author.FirstName = firstName;
            author.LastName = lastName;
            author.DateModified = this.dateTimeProvider.UtcNow;

            _ = this.context.Update(author);
            _ = await this.context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteAuthor(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            _ = this.context.Remove(this.context.Authors.Single(r => r.Id == id));
            _ = await this.context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public virtual async Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken) => await this.context.Authors.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);

    public virtual async Task<bool> AuthorHasReviews(Guid id, CancellationToken cancellationToken) => (await this.ReadAuthorById(id, cancellationToken)).Reviews.Any();

    #endregion Authors

    #region Movies

    public virtual async Task<List<Movie>> ReadAllMovies(CancellationToken cancellationToken) => await this.context.Movies.Include(m => m.Reviews).ThenInclude(r => r.ReviewAuthor).AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<Movie> ReadMovieById(Guid id, CancellationToken cancellationToken) => await this.context.Movies.Where(r => r.Id == id).Include(m => m.Reviews).ThenInclude(r => r.ReviewAuthor).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

    public virtual async Task<bool> MovieExists(Guid id, CancellationToken cancellationToken) => await this.context.Movies.AsNoTracking().AnyAsync(m => m.Id == id, cancellationToken);

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

    public async Task<List<Review>> ReadAllReviews(CancellationToken cancellationToken) => await this.context.Reviews.Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Review> ReadReviewById(Guid id, CancellationToken cancellationToken) => await this.context.Reviews.Where(r => r.Id == id).Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken) => await this.context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);

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
