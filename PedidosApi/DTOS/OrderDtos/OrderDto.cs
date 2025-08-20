using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.Models;

namespace PedidosApi.DTOS.OrderDtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public string ShippingAddress { get; set; } = null!;
        public virtual CustomerDto Customer { get; set; } = null!;
    }
}