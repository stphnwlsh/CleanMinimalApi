namespace CleanMinimalApi.Application.Versions.ReadVersion;

using CleanMinimalApi.Domain.Version;
using MediatR;

public class ReadVersionQuery : IRequest<ApplicationVersion>
{
}
