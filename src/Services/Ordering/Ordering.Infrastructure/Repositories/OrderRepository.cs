using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBaseAsync<Order,long,OrderContext>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) : base(dbContext, unitOfWork)
    {
        
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName) =>
        await FindByCondition(x => x.UserName.Equals(userName)).ToListAsync();

    public Task<Order> GetOrderAsync(long id) => GetByIdAsync(id);

    public async Task<Order> CreateOrderAsync(Order order)
    {
        await CreateAsync(order);
        return order;
    }
    
    public void CreateOrder(Order order) => Create(order);

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        await UpdateAsync(order);
        return order;
    }

    public async Task DeleteOrderAsync(long id)
    {
        var order = await GetOrderAsync(id);
        if (order != null) DeleteAsync(order);
    }
    
    public void DeleteOrder(Order order) => Create(order);
}