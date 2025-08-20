using PedidosApi.DTOS.OrderDetailDtos;
using PedidosApi.Models;

namespace PedidosApi.DTOS.OrderDtos
{
    public class OrderCustomerDto
    {
        public int Id { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public string ShippingAddress { get; set; } = null!;
        public List<OrderDetailDto> OrderDetails { get; set; } = [];
    }
}