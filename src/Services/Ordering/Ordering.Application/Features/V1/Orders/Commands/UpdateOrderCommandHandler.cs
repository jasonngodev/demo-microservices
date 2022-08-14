using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders;

public class UpdateOrderCommandHandler:IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;

    public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ILogger logger)
    {
        _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    private const string MethodName = "UpdateOrderCommandHandler";

    public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var _order = await _repository.GetOrderAsync(request.Id);
        if (_order is null) throw new NotFoundException(nameof(Order), request.Id);
        
        _logger.Information($"BEGIN: {MethodName} - Order: {request.Id}");
        // _order.TotalPrice = request.TotalPrice;
        // _order.FirstName = request.FirstName;
        // _order.EmailAddress = request.EmailAddress;
        // _order.ShippingAddress = request.ShippingAddress;
        // _order.InvoiceAddress = request.InvoiceAddress;
        _order = _mapper.Map(request, _order);
        var _updateOrder = await _repository.UpdateOrderAsync(_order);
        _repository.SaveChangesAsync();//co the bo await do neu co loi da tra luon loi roi

        var result = _mapper.Map<OrderDto>(_updateOrder);
        
        _logger.Information($"END: {MethodName} - Order: {request.Id}");
        return new ApiSuccessResult<OrderDto>(result);
    }
}