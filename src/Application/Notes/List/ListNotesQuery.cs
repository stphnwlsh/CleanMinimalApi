namespace CleanMinimalApi.Application.Notes.List;

using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class ListNotesQuery : IRequest<List<Note>>
{
}
