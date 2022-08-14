using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    [HttpGet("{username}",Name="GetBasket")]
    [ProducesResponseType(typeof(Cart),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> GetBasketByUserName([Required] string username)
    {
        var result = await _basketRepository.GetBasketByUserName(username);
        return Ok(result ?? new Cart());
    }

    [HttpPost(Name = "UpdateBasket")]
    [ProducesResponseType(typeof(Cart),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
    {
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        var result = await _basketRepository.UpdateBasket(cart, options);
        return Ok(result);
    }

    [HttpDelete("{username}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(bool),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
    {
        var result = await _basketRepository.DeleteBasketByUserName(username);
        return Ok(result);
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        var basket = await _basketRepository.GetBasketByUserName(basketCheckout.UserName);
        if (basket == null) return NotFound();
        
        //publish checkout event to EventBus Message
        var eventMessage = _mapper.Map<BasketCheckOutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMessage);
        //remove the basket
        await _basketRepository.DeleteBasketByUserName(basket.UserName);
        
        return Accepted();
    }
}