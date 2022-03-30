using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace Ch7.CustomRouting
{
    public class CurrencyConstraint : IRouteConstraint
    {
        private static readonly IList<string> _currenceis = new List<string> { "EUR", "USD", "GBP" };
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _currenceis.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }
    }
}
