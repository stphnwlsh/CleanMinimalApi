namespace CleanMinimalApi.Infrastructure.Persistance.HardCoded;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Domain.Entities.Notes;

internal class HardCodedNotesContext : INotesContext
{
    private const EntityType Entity = EntityType.Notes;

    private readonly HardCodedNotesDataSource dataSource;

    public HardCodedNotesContext(HardCodedNotesDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public Task<Note> Create(string text, CancellationToken cancellationToken)
    {
        if (text == null || text.Length == 0)
        {
            throw new ActionFailedException(Entity, ActionType.Create, "Note failed to Create in the Data Source");
        }
        else
        {
            return Task.FromResult(this.dataSource.NewNote(text));
        }
    }

    public Task<bool> Delete(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            throw new ActionFailedException(Entity, ActionType.Delete, "Note failed to Delete from the Data Source");
        }
        else if (id <= 5)
        {
            throw new NotFoundException(Entity, "Note was not found in the Data Source");
        }
        else
        {
            var result = this.dataSource.NewBool();

            if (result)
            {
                return Task.FromResult(this.dataSource.NewBool());
            }
            else
            {
                throw new ActionFailedException(Entity, ActionType.Delete, "Note failed to Delete from the Data Source");
            }
        }
    }

    public Task<List<Note>> List(CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<Note>
        {
            this.dataSource.NewNote($"List All Notes - {new Random().Next(0, 100)}"),
            this.dataSource.NewNote($"List All Notes - {new Random().Next(0, 100)}"),
            this.dataSource.NewNote($"List All Notes - {new Random().Next(0, 100)}"),
        });
    }

    public Task<Note> Lookup(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            throw new ActionFailedException(Entity, ActionType.Lookup, "Note failed the Lookup in the Data Source");
        }
        else if (id <= 5)
        {
            throw new NotFoundException(Entity, "Note was not found in the Data Source");
        }
        else
        {
            return Task.FromResult(this.dataSource.NewNote(id, $"Lookup Note - {new Random().Next(0, 100)}"));
        }
    }

    public Task<Note> Update(int id, string text, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            throw new ActionFailedException(Entity, ActionType.Update, "Note failed to Update in the Data Source");
        }
        else if (id <= 5)
        {
            throw new NotFoundException(Entity, "Note was not found in the Data Source");
        }
        else
        {
            return Task.FromResult(this.dataSource.NewNote(id, text));
        }
    }
}
