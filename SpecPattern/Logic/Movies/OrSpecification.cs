using System.Linq.Expressions;

namespace Logic.Movies;

internal sealed class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        BinaryExpression orExpression = Expression.OrElse(leftExpression.Body, rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(orExpression, leftExpression.Parameters.Single());
    }
}