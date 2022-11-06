using System.Linq.Expressions;

namespace Logic.Movies;

internal sealed class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());
    }
}