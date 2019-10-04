using System;
using System.Linq.Expressions;

namespace AccuIT.CommonLayer.Aspects.Extensions
{
    /// <summary>
    /// This class is used to get the expression for the query in collection
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
    }
}
