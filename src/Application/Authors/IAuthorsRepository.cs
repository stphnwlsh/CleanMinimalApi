namespace CleanMinimalApi.Application.Authors;

using System.Threading.Tasks;
using Entities;

public interface IAuthorsRepository
{
    public Task<List<Author>> GetAuthors(CancellationToken cancellationToken);

    public Task<Author> GetAuthorById(Guid id, CancellationToken cancellationToken);

    public Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken);
}
