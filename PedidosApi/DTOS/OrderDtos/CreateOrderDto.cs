namespace PedidosApi.DTOS.OrderDtos
{
    public class CreateOrderDto
    {

        public int CustomerId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Status { get; set; }

        public string ShippingAddress { get; set; } = null!;
        public List<CreateOrderDetailDto> OrderDetail { get; set; } = [];
    }
}