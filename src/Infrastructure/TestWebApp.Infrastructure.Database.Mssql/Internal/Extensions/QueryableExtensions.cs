using System.Linq.Expressions;

namespace TestWebApp.Infrastructure.Database.Mssql.Internal.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> src, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return src.Where(predicate);
            return src;
        }
    }
}
