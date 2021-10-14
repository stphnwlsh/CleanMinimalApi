namespace CleanMinimalApi.Application.Notes.List;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class ListNotesHandler : IRequestHandler<ListNotesQuery, List<Note>>
{
    private readonly INotesContext context;

    public ListNotesHandler(INotesContext context)
    {
        this.context = context;
    }

    public async Task<List<Note>> Handle(ListNotesQuery request, CancellationToken cancellationToken)
    {
        return await this.context.List(cancellationToken);
    }
}
