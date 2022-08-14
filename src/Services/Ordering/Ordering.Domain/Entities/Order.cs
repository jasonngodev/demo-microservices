
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Ordering.Domain.Enums;
using Ordering.Domain.OrderAggregate.Events;

namespace Ordering.Domain.Entities;

public class Order : AudittableEventEntity<long>,IEventEntity//EntityAuditBase<long>
{
    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public  string UserName { get; set; }

    public Guid DocumentNo { get; set; } = Guid.NewGuid();
    
    [Column(TypeName = "decimal(10,2)")]
    public  decimal TotalPrice { get; set; }
    
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public  string FirstName { get; set; }
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public  string LastName { get; set; }
    [Required]
    [EmailAddress]
    [Column(TypeName = "nvarchar(250)")]
    public  string EmailAddress { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public  string ShippingAddress { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public  string InvoiceAddress { get; set; }
    
    public  EOrderStatus Status { get; set; }
    
    [NotMapped]
    public string FullName => FirstName + " " + LastName;
    
    //khi co su kien crud add vao event (thuong don hang khi xoa day vao table nao do luu tru don hang
    public Order AddedOrder()
    {
        AddDomainEvent(new OrderCreatedEvent(Id,UserName,
            TotalPrice,DocumentNo.ToString(),
            FirstName,LastName,
            EmailAddress,
            ShippingAddress,InvoiceAddress));
        return this;
    }

    public Order DeletedOrder()
    {
        AddDomainEvent(new OrderDeletedEvent(Id));
        return this;
    }
}