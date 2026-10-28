namespace CleanMinimalApi.Application.Authors.Queries.SearchAuthors;

using System.Threading;
using System.Threading.Tasks;
using Common.Paging;
using Entities;
using Mediator;

public class SearchAuthorsHandler(IAuthorsRepository repository) : IRequestHandler<SearchAuthorsQuery, Page<Author>>
{
    public async ValueTask<Page<Author>> Handle(SearchAuthorsQuery request, CancellationToken cancellationToken)
    {
        _ = Paging.TryDecode(request.Cursor, out var offset);

        var (items, totalCount) = await repository.SearchAuthors(request.Query, offset, request.Limit, cancellationToken);

        return Paging.ToPage(items, totalCount, offset);
    }
}
