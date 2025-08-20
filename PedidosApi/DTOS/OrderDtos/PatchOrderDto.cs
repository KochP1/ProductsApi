namespace PedidosApi.DTOS.OrderDtos
{
    public class PatchOrderDto
    {
        public string? ShippingAddress { get; set; }
        public string? Status { get; set; }
    }
}