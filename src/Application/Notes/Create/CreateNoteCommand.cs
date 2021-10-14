namespace CleanMinimalApi.Application.Notes.Create;

using System.ComponentModel.DataAnnotations;
using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class CreateNoteCommand : IRequest<Note>
{
    public CreateNoteCommand(string text)
    {
        this.Text = text;
    }

    [Required]
    [MaxLength(300)]
    public string Text { get; set; }
}
