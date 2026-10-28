namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using Common.Paging;
using Entities;
using Mediator;

public class GetAuthorsHandler(IAuthorsRepository repository) : IRequestHandler<GetAuthorsQuery, Page<Author>>
{
    public async ValueTask<Page<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.GetAuthors(request.Query, request.Offset, request.Limit, cancellationToken);

        return new Page<Author>(items, request.Offset, request.Limit, totalCount);
    }
}
