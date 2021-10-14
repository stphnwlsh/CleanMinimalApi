namespace CleanMinimalApi.Application.Common.Interfaces;

using System.Threading.Tasks;
using CleanMinimalApi.Domain.Entities.Notes;

public interface INotesContext
{
    Task<Note> Create(string text, CancellationToken cancellationToken);
    Task<bool> Delete(int id, CancellationToken cancellationToken);
    Task<List<Note>> List(CancellationToken cancellationToken);
    Task<Note> Lookup(int id, CancellationToken cancellationToken);
    Task<Note> Update(int id, string text, CancellationToken cancellationToken);
}
