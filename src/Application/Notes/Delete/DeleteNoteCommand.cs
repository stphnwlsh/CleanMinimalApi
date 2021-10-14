namespace CleanMinimalApi.Application.Notes.Delete;

using System.ComponentModel.DataAnnotations;
using MediatR;

public class DeleteNoteCommand : IRequest<bool>
{
    public DeleteNoteCommand(int id)
    {
        this.Id = id;
    }

    [Required]
    public int Id { get; set; }
}
