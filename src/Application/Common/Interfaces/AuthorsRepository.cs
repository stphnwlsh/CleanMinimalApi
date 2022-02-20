namespace CleanMinimalApi.Application.Common.Interfaces;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Entities;

public interface AuthorsRepository
{
    Task<List<Author>> ReadAllAuthors(CancellationToken cancellationToken);
    Task<Author> ReadAuthorById(Guid id, CancellationToken cancellationToken);
    Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken);
}
