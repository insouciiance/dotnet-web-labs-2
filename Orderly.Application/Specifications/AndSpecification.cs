using AutoMapper.Execution;
using System;
using System.Linq.Expressions;
using E = System.Linq.Expressions.Expression;

namespace Orderly.Application.Specifications;

public class AndSpecification<T>(ISpecification<T> lhs, ISpecification<T> rhs) : ISpecification<T>
{
    public Expression<Func<T, bool>> Expression => And(lhs.Expression, rhs.Expression);

    private static Expression<Func<T, bool>> And(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        ParameterExpression parameter = E.Parameter(typeof(T));

        E body1 = expression1.ReplaceParameters(parameter);
        E body2 = expression2.ReplaceParameters(parameter);

        BinaryExpression andExpression = E.AndAlso(body1, body2);

        return E.Lambda<Func<T, bool>>(andExpression, parameter);
    }
}
