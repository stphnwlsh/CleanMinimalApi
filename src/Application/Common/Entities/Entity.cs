namespace CleanMinimalApi.Application.Common.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
}
