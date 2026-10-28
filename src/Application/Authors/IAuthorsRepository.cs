namespace CleanMinimalApi.Application.Authors;

using Entities;

public interface IAuthorsRepository
{
    public Task<(List<Author> Items, int TotalCount)> GetAuthors(
        string? query,
        int offset,
        int limit,
        CancellationToken cancellationToken);

    public Task<Author?> GetAuthorById(Guid id, CancellationToken cancellationToken);

    public Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken);
}
