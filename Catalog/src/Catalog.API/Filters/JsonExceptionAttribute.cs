using Catalog.API.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Catalog.API.Filters
{
    public class JsonExceptionAttribute : TypeFilterAttribute
    {
        public JsonExceptionAttribute() : base(typeof(HttpCustomExceptionfilterIml))
        {
        }

        private class HttpCustomExceptionfilterIml : IExceptionFilter
        {
            private readonly IWebHostEnvironment _env;
            private readonly ILogger<HttpCustomExceptionfilterIml> _logger;

            public HttpCustomExceptionfilterIml(IWebHostEnvironment env, ILogger<HttpCustomExceptionfilterIml> loger)
            {
                this._env = env;
                this._logger = loger;
            }

            public void OnException(ExceptionContext context)
            {
                var eventId = new EventId(context.Exception.HResult);

                _logger.LogError(eventId, context.Exception, context.Exception.Message);

                var json = new JsonErrorPayload { EventId = eventId.Id };

                if (_env.IsDevelopment())
                {
                    json.DetailedMessage = context.Exception;
                }

                var exceptionObject = new ObjectResult(json)
                {
                    StatusCode = 500
                };

                context.Result = exceptionObject;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }

}
