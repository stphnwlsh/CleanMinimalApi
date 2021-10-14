namespace CleanMinimalApi.Application.Notes.Lookup;

using System.ComponentModel.DataAnnotations;
using CleanMinimalApi.Domain.Entities.Notes;
using MediatR;

public class LookupNoteQuery : IRequest<Note>
{
    public LookupNoteQuery(int id)
    {
        this.Id = id;
    }

    [Required]
    public int Id { get; set; }
}
