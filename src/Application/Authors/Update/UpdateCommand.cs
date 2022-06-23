namespace CleanMinimalApi.Application.Authors.Update;

using MediatR;

public class UpdateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
