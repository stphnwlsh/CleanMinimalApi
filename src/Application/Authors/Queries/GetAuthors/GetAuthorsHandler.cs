namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, List<Author>>
{
    private readonly IAuthorsRepository repository;

    public GetAuthorsHandler(IAuthorsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.GetAuthors(cancellationToken);
    }
}
