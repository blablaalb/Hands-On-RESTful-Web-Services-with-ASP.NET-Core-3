using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Ch7.Filters
{
    public class CustomExceptionAttribute : TypeFilterAttribute
    {
        public CustomExceptionAttribute() : base(typeof(HttpCustomExceptionFilterIml))
        {
        }

        private class HttpCustomExceptionFilterIml : IExceptionFilter
        {
            private readonly IWebHostEnvironment _env;
            private readonly ILogger<HttpCustomExceptionFilterIml> _logger;

            public HttpCustomExceptionFilterIml(IWebHostEnvironment webHost, ILogger<HttpCustomExceptionFilterIml> logger)
            {
                _env = webHost;
                _logger = logger;
            }

            public void OnException(ExceptionContext context)
            {
                _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);
                var json = new JsonErrorPayload
                {
                    Message = new[] { "An error occured. Try it again." }
                };

                if (_env.IsDevelopment())
                {
                    json.DetailedMessage = context.Exception;
                }

                var exceptionObject = new ObjectResult(json) { StatusCode = 500 };
                context.Result = exceptionObject;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        public class JsonErrorPayload
        {
            public string[] Message { get; set; }
            public object DetailedMessage { get; set; }
        }
    }
}
