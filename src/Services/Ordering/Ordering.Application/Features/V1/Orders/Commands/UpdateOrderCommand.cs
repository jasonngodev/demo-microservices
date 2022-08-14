using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders;

public class UpdateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>, IMappFrom<Order>
{
    public long Id { get; private set; }

    public void SetId(long id)
    {
        Id = id;
    }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateOrderCommand, Order>()
            .ForMember(dest => dest.Status, opts => opts.Ignore())
            .IgnoreAllNonExisting();//khong duoc update status do update binh thuong
    }
    
    // public  string UserName { get; set; }
    // public  decimal TotalPrice { get; set; }
    //
    // public  string FirstName { get; set; }
    // public  string LastName { get; set; }
    // public  string EmailAddress { get; set; }
    //
    // public  string ShippingAddress { get; set; }
    // public  string InvoiceAddress { get; set; }
}