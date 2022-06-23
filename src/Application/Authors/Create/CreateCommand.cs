namespace CleanMinimalApi.Application.Authors.Create;

using Entities;
using MediatR;

public class CreateCommand : IRequest<Author>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
