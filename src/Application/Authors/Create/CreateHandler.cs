namespace CleanMinimalApi.Application.Authors.Create;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Entities;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand, Author>
{
    private readonly AuthorsRepository authorsRepository;

    public CreateHandler(AuthorsRepository authorsRepository)
    {
        this.authorsRepository = authorsRepository;
    }

    public async Task<Author> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        return await this.authorsRepository.CreateAuthor(request.FirstName, request.LastName, cancellationToken);
    }
}
