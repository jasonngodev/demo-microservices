using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders;

public class DeleteOrderCommandHandler:IRequestHandler<DeleteOrderCommand, ApiResult<bool>>
{
    // private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;

    public DeleteOrderCommandHandler(IOrderRepository repository, ILogger logger)
    {
        // _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    private const string MethodName = "DeleteOrderCommandHandler";
    
    public async Task<ApiResult<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {MethodName} - Id: {request.Id}");
        var _orderEntities = await _repository.GetByIdAsync(request.Id);
        if (_orderEntities != null)
        {
            //await _repository.DeleteOrderAsync(_orderEntities.Id);
            _repository.DeleteOrder(_orderEntities);
            _orderEntities.DeletedOrder();
            await _repository.SaveChangesAsync();
            
            _logger.Information($"END: {MethodName} deleted successfully - Id: {request.Id}");
            
            return new ApiSuccessResult<bool>(true, "Deleted");
        }

        _logger.Information($"END: {MethodName} failed - Id: {request.Id}");
        // return Unit.Value;
        return new ApiErrorResult<bool>("No record");
    }
}