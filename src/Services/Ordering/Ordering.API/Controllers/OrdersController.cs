using System.ComponentModel.DataAnnotations;
using System.Net;
using Contracts.Messages;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISmtpEmailService _smtpEmailService;
    // private readonly IMessageProducer _messageProducer;

    public OrdersController(IMediator mediator, ISmtpEmailService smtpEmailService)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _smtpEmailService = smtpEmailService;
        // _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
    }

    private static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
        public const string CreateOrder = nameof(CreateOrder);
        public const string UpdateOrder = nameof(UpdateOrder);
        public const string DeleteOrder = nameof(DeleteOrder);
    }

    [HttpGet("{username}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string username)
    {
        var query = new GetOrdersQuery(username);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost(Name = RouteNames.CreateOrder)]
    [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody]CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        // _messageProducer.SendMessage(result);
        return Ok(result);
    }
    
    [HttpPut("{id:long}",Name = RouteNames.UpdateOrder)]
    [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderDto>> UpdateOrder([Required]long id,[FromBody]UpdateOrderCommand command)
    {
        command.SetId(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{id:long}",Name = RouteNames.DeleteOrder)]
    // [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiResult<bool>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteOrder([Required]long id)
    {
        var command = new DeleteOrderCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("test-email")]
    public async Task<IActionResult> TestMail()
    {
        var message = new MailRequest
        {
            Body = "hello",
            Subject = "Test",
            ToAddress = "jasonngo0880@gmail.com"
        };

        await _smtpEmailService.SendEmailAsync(message);

        return Ok();
    }
}