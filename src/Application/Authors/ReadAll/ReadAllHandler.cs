namespace CleanMinimalApi.Application.Authors.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Author>>
{
    private readonly AuthorsRepository repository;

    public ReadAllHandler(AuthorsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Author>> Handle(ReadAllQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.ReadAllAuthors(cancellationToken);
    }
}
