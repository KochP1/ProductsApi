namespace PedidosApi.DTOS.OrderDtos
{
    public class PatchOrderDetailDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}