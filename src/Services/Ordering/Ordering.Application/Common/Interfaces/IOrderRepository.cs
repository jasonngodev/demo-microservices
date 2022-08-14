using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces;

public interface IOrderRepository : IRepositoryBaseAsync<Order,long>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    Task<Order> GetOrderAsync(long id);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(long id);
    void CreateOrder(Order order);
    void DeleteOrder(Order order);
}