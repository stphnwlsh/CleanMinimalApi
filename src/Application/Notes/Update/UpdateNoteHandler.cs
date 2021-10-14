namespace CleanMinimalApi.Application.Notes.Update;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class UpdateNoteHandler : IRequestHandler<UpdateNoteCommand, Note>
{
    private readonly INotesContext context;

    public UpdateNoteHandler(INotesContext context)
    {
        this.context = context;
    }

    public async Task<Note> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        return await this.context.Update(request.Id, request.Text.Trim(), cancellationToken);
    }
}
