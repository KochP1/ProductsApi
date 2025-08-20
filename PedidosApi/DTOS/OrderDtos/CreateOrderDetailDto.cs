namespace PedidosApi.DTOS.OrderDtos
{
    public class CreateOrderDetailDto
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Subtotal { get; set; }
    }
}