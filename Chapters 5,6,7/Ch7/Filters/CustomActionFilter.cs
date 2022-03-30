using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Ch7.Filters
{
    public class CustomActionFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public CustomActionFilter(ILogger logger)
        {
            this._logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
