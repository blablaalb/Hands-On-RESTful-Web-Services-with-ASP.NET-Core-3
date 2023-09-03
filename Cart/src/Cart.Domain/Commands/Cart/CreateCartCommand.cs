using Cart.Domain.Responses.Cart;
using MediatR;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Cart.Domain.Commands.Cart
{
    public class CreateCartCommand : IRequest<CartExtendedResponse>
    {
        public string[] ItemsIds { get; set; }

        public string UserEmail { get; set; }
    }

    public class TestResponse
    {
        public string Message { get; set; }
    }

    public class TestCommand : IRequest<TestResponse> { }

    public class TestHandler : IRequestHandler<TestCommand, TestResponse>
    {
        public Task<TestResponse> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            var result =  Task.FromResult<TestResponse>(new TestResponse { Message = "first handler" });
            return result;
        }
    }

    public class TestHandlerSecond : IRequestHandler<TestCommand, TestResponse>
    {
        public Task<TestResponse> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            var result =  Task.FromResult<TestResponse>(new TestResponse { Message = "second handler" });
            return result;
        }
    }
}