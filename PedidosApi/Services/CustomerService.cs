using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.Interfaces;
using PedidosApi.Models;

namespace PedidosApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CustomerService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            return customer;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await context.Customers.ToListAsync();
            return mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerWithOrdersDto> GetCustomerWithOrdersAsync(int id)
        {
            var customer = await context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderDetails)
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<CustomerWithOrdersDto>(customer);
        }

        public async Task<CustomerDto> CreateCustomer(CustomerDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<int> DeleteCustomer(int id)
        {
            var record = await context.Customers.Where(x => x.Id == id).ExecuteDeleteAsync();
            return record;
        }

        public async Task<bool> PatchCustomer(JsonPatchDocument<PatchCustomerDto> patchDoc, Customer customerDb)
        {
            try
            {
                var customerPatchDto = mapper.Map<PatchCustomerDto>(customerDb);
                
                patchDoc.ApplyTo(customerPatchDto);
                
                mapper.Map(customerPatchDto, customerDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}