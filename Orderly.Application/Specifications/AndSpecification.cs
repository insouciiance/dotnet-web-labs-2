namespace Orderly.Application.Specifications;

public class AndSpecification<T>(ISpecification<T> lhs, ISpecification<T> rhs) : ISpecification<T>
{
    public bool IsSatisfiedBy(T item) => lhs.IsSatisfiedBy(item) && rhs.IsSatisfiedBy(item);
}
