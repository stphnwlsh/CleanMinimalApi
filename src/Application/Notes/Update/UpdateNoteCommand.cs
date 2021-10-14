namespace CleanMinimalApi.Application.Notes.Update;

using System.ComponentModel.DataAnnotations;
using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class UpdateNoteCommand : IRequest<Note>
{
    public UpdateNoteCommand(int id, string text)
    {
        this.Id = id;
        this.Text = text;
    }

    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string Text { get; set; }
}
