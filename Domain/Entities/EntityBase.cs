namespace Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}