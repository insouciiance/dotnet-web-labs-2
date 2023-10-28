﻿namespace Orderly.Application.Specifications;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}
