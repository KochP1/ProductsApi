using PedidosApi.DTOS.OrderDtos;

namespace PedidosApi.Interfaces
{
    public interface IOrderService
    {
        public Task<CreateOrderDto> CreateOrder(CreateOrderDto newOrder);
        public Task<bool> ValidateCustomer(CreateOrderDto createOrderDto);
        public Task<bool> ValidateProducts(CreateOrderDto createOrderDto);
        public Task<int> DeleteOrder(int id);
    }
}