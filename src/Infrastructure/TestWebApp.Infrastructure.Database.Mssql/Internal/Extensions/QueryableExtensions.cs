namespace TestWebApp.Infrastructure.Database.Mssql.Internal.Extensions
{
    using System.Linq.Expressions;

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
