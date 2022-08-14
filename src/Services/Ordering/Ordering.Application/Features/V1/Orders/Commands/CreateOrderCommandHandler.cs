using AutoMapper;
using Contracts.Services;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.Application.Features.V1.Orders;

public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;
    private readonly ISmtpEmailService _emailService;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ILogger logger, ISmtpEmailService emailService)
    {
        _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }
    
    private const string MethodName = "CreateOrderCommandHandler";

    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {MethodName} - Username: {request.UserName}");
        // var _addNew = new Order
        // {
        //     UserName = request.UserName,
        //     TotalPrice = request.TotalPrice,
        //     FirstName = request.FirstName,
        //     LastName = request.LastName,
        //     EmailAddress = request.EmailAddress,
        //     ShippingAddress = request.ShippingAddress,
        //     InvoiceAddress = request.InvoiceAddress
        // };
        var orderEntity = _mapper.Map<Order>(request);
        //await _repository.CreateOrderAsync(orderEntity);
        _repository.CreateOrder(orderEntity);
        //trigger
        orderEntity.AddedOrder();
        await _repository.SaveChangesAsync();
        
        //public event to know
        
        
        _logger.Information($"ORDER: {MethodName} - Id: {orderEntity.Id}");

        // SendEmailAsync(orderEntity, cancellationToken);
        
        _logger.Information($"END: {MethodName} - Username: {request.UserName}");
        return new ApiSuccessResult<long>(orderEntity.Id);
    }

    private async Task SendEmailAsync(Order order, CancellationToken cancellationToken)
    {
        var emailRequest = new MailRequest
        {
            ToAddress = order.EmailAddress,
            Body = "Order sent successfully",
            Subject = "Order was created"
        };

        try
        {
            await _emailService.SendEmailAsync(emailRequest, cancellationToken);
            _logger.Information($"Sent Created Order to email: {order.EmailAddress}");
        }
        catch (Exception e)
        {
            _logger.Error($"Order {order.Id} failed due to an error with email {e.Message}");
        }
    }
}