using Microsoft.AspNetCore.JsonPatch;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.DTOS.OrderDtos;
using PedidosApi.Models;

namespace PedidosApi.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> GetAllOrders();
        public Task<OrderDto> GetOrderById(int id);
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<OrderDetail> GetOrderDetailByIdAsync(int id);
        public Task<IEnumerable<OrderWithDetailDto>> GetOrderCollection();
        public Task<OrderWithDetailDto> CreateOrder(CreateOrderDto newOrder);
        public Task<bool> ValidateCustomer(CreateOrderDto createOrderDto);
        public Task<bool> ValidateProducts(CreateOrderDto createOrderDto);
        public Task<int> DeleteOrder(int id);
        public Task<bool> PatchOrder(JsonPatchDocument<PatchOrderDto> patchDoc, Order orderDb);
        public Task<bool> PatchOrderDetail(JsonPatchDocument<PatchOrderDetailDto> patchDoc, OrderDetail orderDb);
    }
}