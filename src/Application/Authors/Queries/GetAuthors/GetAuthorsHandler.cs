namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using Common.Paging;
using Entities;
using Mediator;

public class GetAuthorsHandler(IAuthorsRepository repository) : IRequestHandler<GetAuthorsQuery, Page<AuthorSummary>>
{
    public async ValueTask<Page<AuthorSummary>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.GetAuthors(request.Query, request.Offset, request.Limit, cancellationToken);

        return new Page<AuthorSummary>(items, request.Offset, request.Limit, totalCount);
    }
}
