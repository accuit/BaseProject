using System.Collections.Generic;
using System.Linq.Expressions;

namespace AccuIT.CommonLayer.Aspects.Extensions
{
    /// <summary>
    /// This expression class is used to validate the expression result in an object and binding of associated parameters
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;
        /// <summary>
        /// Initialize constructor to associate dictionary mappings with expression
        /// </summary>
        /// <param name="map"></param>
        private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        /// <summary>
        /// This method is used to replace the parameters 
        /// </summary>
        /// <param name="map">expression maps dictionary</param>
        /// <param name="exp">expression</param>
        /// <returns>returns expression</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        /// <summary>
        /// This method is to get the value of expression
        /// </summary>
        /// <param name="node">parameter expression object</param>
        /// <returns>returns expression</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;
            if (_map.TryGetValue(node, out replacement))
            {
                node = replacement;
            }
            return base.VisitParameter(node);
        }
    }
}
