using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.DTOS.OrderDetailDtos;
using PedidosApi.DTOS.OrderDtos;
using PedidosApi.Interfaces;
using PedidosApi.Models;

namespace PedidosApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await context.Orders.Include(x => x.Customer).ToListAsync();
            return mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderById(int id)
        {
            var order = await context.Orders.Include(x => x.Customer).Where(x => x.Id == id).FirstOrDefaultAsync();
            return mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderWithDetailDto>> GetOrderCollection()
        {
            var orders = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();

            return orders.Select(order => new OrderWithDetailDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                Customer = mapper.Map<CustomerDto>(order.Customer),
                OrderDetails = mapper.Map<List<OrderDetailDto>>(order.OrderDetails)
            });
        }

        public async Task<CreateOrderDto> CreateOrder(CreateOrderDto newOrder)
        {
            var order = mapper.Map<Order>(newOrder);
            order.OrderDate = DateTime.UtcNow;

            foreach (var detail in order.OrderDetails)
            {
                detail.Subtotal = detail.Quantity * detail.UnitPrice;
            }

            order.TotalAmount = order.OrderDetails.Sum(od => od.Subtotal ?? 0);

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var response = mapper.Map<CreateOrderDto>(order);
            return response;
        }

        public async Task<bool> ValidateCustomer(CreateOrderDto createOrderDto)
        {
            var customerExists = await context.Customers.Where(c => c.Id == createOrderDto.CustomerId).FirstOrDefaultAsync();
            if (customerExists is null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateProducts(CreateOrderDto createOrderDto)
        {
            var productIds = createOrderDto.OrderDetail.Select(od => od.ProductId).ToList();
            var existingProducts = await context.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            var invalidProducts = productIds.Except(existingProducts).ToList();
            if (invalidProducts.Any())
            {
                return false;
            }
            return true;
        }

        public async Task<int> DeleteOrder(int id){
            var record = await context.Orders.Where(x => x.Id == id).ExecuteDeleteAsync();
            return record;
        }
    }
}
