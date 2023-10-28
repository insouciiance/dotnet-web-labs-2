namespace Orderly.Application.Entities;

public interface IEntity<TKey>
    where TKey : notnull
{
    TKey Id { get; }
}
