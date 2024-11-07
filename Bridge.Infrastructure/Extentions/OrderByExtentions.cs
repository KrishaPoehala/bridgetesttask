using System.Linq.Expressions;

namespace Bridge.Infrastructure.Extentions;

public static class OrderByExtentions
{
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource,TKey>> selector,
        bool order)
    {
        if (order)
        {
            return source.OrderBy(selector);
        }

        return source.OrderByDescending(selector);
    }
}
