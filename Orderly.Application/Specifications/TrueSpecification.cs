namespace Orderly.Application.Specifications;

public class TrueSpecification<T> : ISpecification<T>
{
    public bool IsSatisfiedBy(T entity) => true;
}
