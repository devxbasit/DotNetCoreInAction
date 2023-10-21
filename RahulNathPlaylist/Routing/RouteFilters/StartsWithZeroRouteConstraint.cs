using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Routing.RouteFilters
{
    public class StartsWithZeroRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out object routeKeyValue))
            {
                if (routeKeyValue is string value)
                {
                    return value.StartsWith("0");
                }
            }

            return false;
        }
    }
}