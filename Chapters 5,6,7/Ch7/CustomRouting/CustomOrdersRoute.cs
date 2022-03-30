using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace Ch7.CustomRouting
{
    public class CustomOrdersRoute : Attribute, IRouteTemplateProvider
    {
        public string Name => "Orders_route";

        public int? Order { get; set; }

        public string Template => "api/orders";
    }
}
