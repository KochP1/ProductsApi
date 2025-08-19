using Microsoft.AspNetCore.JsonPatch;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.Models;

namespace PedidosApi.Interfaces
{
    public interface ICustomerService
    {
        public Task<CustomerDto> GetCustomerByIdAsync(int id);
        public Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        public Task<CustomerDto> CreateCustomer(CustomerDto customerDto);
        public Task<int> DeleteCustomer(int id);
        public Task<bool> PatchCustomer(JsonPatchDocument<PatchCustomerDto> patchDoc, Customer customerDb);
        public Task<Customer> GetCustomerById(int id);
    }
}