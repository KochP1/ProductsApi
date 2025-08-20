using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.DTOS.DeliveryDtos;
using PedidosApi.DTOS.EmployeeDtos;
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
            var order = await context.Orders.Include(x => x.Deliveries)
            .Include(x => x.Customer).Where(x => x.Id == id).FirstOrDefaultAsync();
            return mapper.Map<OrderDto>(order);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await context.Orders.FindAsync(id);
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int id)
        {
            return await context.OrderDetails.FindAsync(id);
        }

        public async Task<IEnumerable<OrderWithDetailDto>> GetOrderCollection()
        {
            var orders = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Deliveries)
                    .ThenInclude(d => d.DeliveryStatusHistories)
                .Include(o => o.Deliveries)
                    .ThenInclude(d => d.DeliveryPerson)
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
                OrderDetails = mapper.Map<List<OrderDetailDto>>(order.OrderDetails),
                Delivery = order.Deliveries.FirstOrDefault() != null ? new DeliveryDto
                {
                    Id = order.Deliveries.FirstOrDefault()!.Id,
                    OrderId = order.Deliveries.FirstOrDefault()!.OrderId,
                    EstimatedDelivery = order.Deliveries.FirstOrDefault()!.EstimatedDelivery,
                    ActualDelivery = order.Deliveries.FirstOrDefault()!.ActualDelivery,
                    DeliveryAddress = order.Deliveries.FirstOrDefault()!.DeliveryAddress,
                    CurrentStatus = order.Deliveries.FirstOrDefault()!.CurrentStatus,
                    DeliveryPerson = mapper.Map<EmployeeDto>(order.Deliveries.FirstOrDefault()!.DeliveryPerson),
                    DeliveryStatus = order.Deliveries.FirstOrDefault()!.DeliveryStatusHistories != null ?
                        mapper.Map<DeliveryStatusDto>(order.Deliveries.FirstOrDefault()!.DeliveryStatusHistories
                            .OrderByDescending(dsh => dsh.StatusDate)
                            .FirstOrDefault()) : null
                } : null
            });
        }

        public async Task<OrderWithDetailDto> CreateOrder(CreateOrderDto newOrder)
        {
            // 1. Crear Order
            var order = new Order
            {
                CustomerId = newOrder.CustomerId,
                ShippingAddress = newOrder.ShippingAddress,
                OrderDate = DateTime.UtcNow,
                Status = "Pendiente"
            };

            // 2. Crear OrderDetails
            order.OrderDetails = newOrder.OrderDetail.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                Subtotal = od.Quantity * od.UnitPrice
            }).ToList();

            order.TotalAmount = order.OrderDetails.Sum(od => od.Subtotal ?? 0);

            // 3. Crear Delivery
            var delivery = new Delivery
            {
                DeliveryAddress = newOrder.Delivery.DeliveryAddress,
                DeliveryPersonId = newOrder.Delivery.DeliveryPersonId,
                EstimatedDelivery = newOrder.Delivery.EstimatedDelivery ?? DateTime.UtcNow.AddDays(3),
                CurrentStatus = newOrder.Delivery.DeliveryStatus.Status,
                DeliveryStatusHistories = new List<DeliveryStatusHistory>()
                {
                    new DeliveryStatusHistory
                    {
                        Status = newOrder.Delivery.DeliveryStatus.Status,
                        StatusDate = DateTime.UtcNow,
                        Notes = newOrder.Delivery.DeliveryStatus.Notes
                    }
                }
            };

            order.Deliveries = new List<Delivery> { delivery };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            return mapper.Map<OrderWithDetailDto>(order);
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

        public async Task<int> DeleteOrder(int id)
        {
            var record = await context.Orders.Where(x => x.Id == id).ExecuteDeleteAsync();
            return record;
        }

        public async Task<bool> PatchOrder(JsonPatchDocument<PatchOrderDto> patchDoc, Order orderDb)
        {
            try
            {
                var orderPatchDto = mapper.Map<PatchOrderDto>(orderDb);

                patchDoc.ApplyTo(orderPatchDto);

                mapper.Map(orderPatchDto, orderDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<bool> PatchOrderDetail(JsonPatchDocument<PatchOrderDetailDto> patchDoc, OrderDetail orderDb)
        {
            try
            {
                var orderPatchDto = mapper.Map<PatchOrderDetailDto>(orderDb);
                
                patchDoc.ApplyTo(orderPatchDto);
                
                mapper.Map(orderPatchDto, orderDb);

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
