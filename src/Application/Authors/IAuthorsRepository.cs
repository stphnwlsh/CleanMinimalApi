namespace CleanMinimalApi.Application.Authors;

using System.Threading.Tasks;
using Entities;

public interface IAuthorsRepository
{
    Task<List<Author>> GetAuthors(CancellationToken cancellationToken);

    Task<Author> GetAuthorById(Guid id, CancellationToken cancellationToken);

    Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken);
}
