namespace CleanMinimalApi.Application.Notes.Create;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class CreateNoteHandler : IRequestHandler<CreateNoteCommand, Note>
{
    private readonly INotesContext context;

    public CreateNoteHandler(INotesContext context)
    {
        this.context = context;
    }

    public async Task<Note> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        return await this.context.Create(request.Text.Trim(), cancellationToken);
    }
}
