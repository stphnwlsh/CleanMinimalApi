namespace CleanMinimalApi.Domain.Entities.Notes;

public class Note
{
    public int Id { get; set; }
    public string Text { get; set; }

    public Note()
    {
        this.Id = default;
        this.Text = string.Empty;
    }

    public Note(string text)
    {
        this.Id = new Random().Next(0, 100);
        this.Text = text;
    }

    public Note(int id, string text)
    {
        this.Id = id;
        this.Text = text;
    }
}
