namespace EventBus.Messages.IntegrationEvents.Interfaces;

public interface IBasketCheckOutEvent:IIntegrationEvent
{  
    string UserName { get; set; }
      decimal TotalPrice { get; set; }
      string FirstName { get; set; }
      string LastName { get; set; }
      string EmailAddress { get; set; }
     string ShippingAddress { get; set; }
     string InvoiceAddress { get; set; }
}