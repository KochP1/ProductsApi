using PedidosApi.DTOS.OrderDtos;

namespace PedidosApi.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> GetAllOrders();
        public Task<OrderDto> GetOrderById(int id);
        public Task<IEnumerable<OrderWithDetailDto>> GetOrderCollection();
        public Task<OrderWithDetailDto> CreateOrder(CreateOrderDto newOrder);
        public Task<bool> ValidateCustomer(CreateOrderDto createOrderDto);
        public Task<bool> ValidateProducts(CreateOrderDto createOrderDto);
        public Task<int> DeleteOrder(int id);
    }
}