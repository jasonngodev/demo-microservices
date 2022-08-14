using Contracts.Common.Events;

namespace Ordering.Domain.OrderAggregate.Events;

public class OrderCreatedEvent:BaseEvent
{
    public  long Id { get; set; }
    public  string UserName { get; set; }
    
    public  string DocumentNo { get; set; }
    public  decimal TotalPrice { get; set; }
    public  string FirstName { get; set; }
    public  string LastName { get; set; }
    public  string EmailAddress { get; set; }
    public  string ShippingAddress { get; set; }
    public  string InvoiceAddress { get; set; }

    public OrderCreatedEvent(long id, string userName, decimal totalPrice, string documentNo, string firstName, string lastName, string emailAddress, string shippingAddress, string invoiceAddress)
    {
        Id = id;
        TotalPrice = totalPrice;
        UserName = userName;
        DocumentNo = documentNo;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        ShippingAddress = shippingAddress;
        InvoiceAddress = invoiceAddress;
    }
}