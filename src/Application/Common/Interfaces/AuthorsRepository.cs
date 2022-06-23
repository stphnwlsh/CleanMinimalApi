namespace CleanMinimalApi.Application.Common.Interfaces;

using System.Threading.Tasks;
using Entities;

public interface AuthorsRepository
{
    #region CRUD

    Task<Author> CreateAuthor(string firstName, string lastName, CancellationToken cancellationToken);
    Task<List<Author>> ReadAllAuthors(CancellationToken cancellationToken);
    Task<Author> ReadAuthorById(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateAuthor(Guid id, string firstName, string lastName, CancellationToken cancellationToken);
    Task<bool> DeleteAuthor(Guid id, CancellationToken cancellationToken);

    #endregion CRUD

    Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken);
    Task<bool> AuthorHasReviews(Guid id, CancellationToken cancellationToken);
}
