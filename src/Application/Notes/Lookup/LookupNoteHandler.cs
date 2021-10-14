namespace CleanMinimalApi.Application.Notes.Lookup;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class LookupNoteHandler : IRequestHandler<LookupNoteQuery, Note>
{
    private readonly INotesContext context;

    public LookupNoteHandler(INotesContext context)
    {
        this.context = context;
    }

    public async Task<Note> Handle(LookupNoteQuery request, CancellationToken cancellationToken)
    {
        return await this.context.Lookup(request.Id, cancellationToken);
    }
}
