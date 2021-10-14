namespace CleanMinimalApi.Application.Notes.Delete;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Interfaces;
using MediatR;

public class DeleteNoteHandler : IRequestHandler<DeleteNoteCommand, bool>
{
    private readonly INotesContext context;

    public DeleteNoteHandler(INotesContext context)
    {
        this.context = context;
    }

    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        return await this.context.Delete(request.Id, cancellationToken);
    }
}
