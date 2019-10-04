using System;
using System.Linq;
using System.Linq.Expressions;

namespace AccuIT.CommonLayer.Aspects.Extensions
{
    /// <summary>
    /// This class is used for the extension method for LINQ Expressions and anonymous functions
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// This method is used to compose expression to build multiple functions and lambda expressions
        /// </summary>
        /// <typeparam name="T">type of target object</typeparam>
        /// <param name="first">first expression</param>
        /// <param name="second">second expression</param>
        /// <param name="merge">expression to merge</param>
        /// <returns>returns expression</returns>
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        /// <summary>
        /// This expression method is used to logically append any expression with AND operator
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="first">first expression</param>
        /// <param name="second">second expression</param>
        /// <returns>result expression</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        /// <summary>
        /// This expression method is used to logically append any expression with OR operator
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="first">first expression</param>
        /// <param name="second">second expression</param>
        /// <returns>result expression</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}
