using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerRepositoryAsync:RepositoryQueryBase<Entities.Customer,int,CustomerContext>,ICustomerRepository
{
    public CustomerRepositoryAsync(CustomerContext dbContext) : base(dbContext)
    {
        
    }

    public Task<Entities.Customer> GetCustomerByUserNameAsync(string username) =>
        FindByCondition(x => x.UserName.Equals(username)).SingleOrDefaultAsync();
    
    public async Task<IEnumerable<Entities.Customer>> GetCustomersAsync() => await FindAll().ToListAsync();
}