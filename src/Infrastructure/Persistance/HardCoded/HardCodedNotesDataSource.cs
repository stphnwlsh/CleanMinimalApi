namespace CleanMinimalApi.Infrastructure.Persistance.HardCoded;

using CleanMinimalApi.Domain.Entities.Notes;

internal class HardCodedNotesDataSource
{
    public virtual Note NewNote(string text)
    {
        return new Note(text);
    }

    public virtual Note NewNote(int id, string text)
    {
        return new Note(id, text);
    }

    public virtual bool NewBool()
    {
        return true;
    }
}
