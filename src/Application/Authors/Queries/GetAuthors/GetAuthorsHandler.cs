namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using Mediator;

public class GetAuthorsHandler(IAuthorsRepository repository) : IRequestHandler<GetAuthorsQuery, List<Author>>
{
    public async ValueTask<List<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAuthors(cancellationToken);
    }
}
