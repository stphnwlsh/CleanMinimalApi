namespace CleanMinimalApi.Application.Authors.Create;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand, Author>
{
    private readonly IAuthorsRepository authorsRepository;

    public CreateHandler(IAuthorsRepository authorsRepository) => this.authorsRepository = authorsRepository;

    public async Task<Author> Handle(CreateCommand request, CancellationToken cancellationToken) => await this.authorsRepository.CreateAuthor(request.FirstName, request.LastName, cancellationToken);
}
