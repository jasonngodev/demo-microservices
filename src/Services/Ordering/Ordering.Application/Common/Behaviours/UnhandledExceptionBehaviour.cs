using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : 
    IPipelineBehavior<TRequest,TResponse>
    where TRequest: IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }


    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(e,"Application Request: Unhandled exception for request {Name} {@Request}",requestName, request);
            throw;
        }
    }
}