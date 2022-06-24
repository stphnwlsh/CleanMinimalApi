namespace CleanMinimalApi.Application.Versions.Queries.GetVersion;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetVersionQuery : IRequest<ApplicationVersion>
{
}
