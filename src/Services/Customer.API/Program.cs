using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API;
using Customer.API.Controllers;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Customer;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
  
// Add services to the container.
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(
        options => options.UseNpgsql(connectionString));

    builder.Services.AddScoped<ICustomerRepository, CustomerRepositoryAsync>()
        .AddScoped(typeof(IRepositoryQueryBase<,,>), typeof(RepositoryQueryBase<,,>))
        .AddScoped<ICustomerService, CustomerService>();
    
    var app = builder.Build();

    app.MapGet("/", () => $"Welcome to {builder.Environment.ApplicationName}!");
    // app.MapGet("/api/customers", async (ICustomerService customerService) => await customerService.GetCustomersAsync());
    app.MapCustomersAPI();
    // app.MapPost("/api/customers", async (Customer.API.Entities.Customer customer, ICustomerRepository customerRepository) =>
    // {
    //     customerRepository.CreateAsync(customer);
    //     customerRepository.SaveChangesAsync();
    // });
    // app.MapPut("/api/customers", async (CreateOrUpdateCustomerDto customerDto, ICustomerRepository customerRepository) =>
    // {
    //     var customer = await customerRepository
    //         .FindByCondition(x => x.Id.Equals(customerDto.Id))
    //         .SingleOrDefaultAsync();
    //     
    //     if (customer==null) return Results.NotFound();
    //
    //     customer.FirstName = customerDto.FirstName;
    //     customer.LastName = customerDto.LastName;
    //
    //     await customerRepository.UpdateAsync(customer);
    //     await customerRepository.SaveChangesAsync();
    //
    //     return Results.Ok(customer);
    // });
    // app.MapDelete("/api/customers/{id}", async (int id, ICustomerRepository customerRepository) =>
    // {
    //     var customer = await customerRepository
    //         .FindByCondition(x => x.Id.Equals(id))
    //         .SingleOrDefaultAsync();
    //     
    //     if (customer == null) return Results.NotFound();
    //
    //     await customerRepository.DeleteAsync(customer);
    //     await customerRepository.SaveChangesAsync();
    //
    //     return Results.NoContent();
    // });
    
// Configure the HTTP request pipeline.
    // if (app.Environment.IsDevelopment())
    // {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
            $"{builder.Environment.ApplicationName} v1"));
    });
    // }

    // app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.SeedCustomerData()
        .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Customer API complete");
    Log.CloseAndFlush();
}