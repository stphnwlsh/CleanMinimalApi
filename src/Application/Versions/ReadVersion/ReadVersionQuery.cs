namespace CleanMinimalApi.Application.Versions.ReadVersion;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadVersionQuery : IRequest<ApplicationVersion>
{
}
