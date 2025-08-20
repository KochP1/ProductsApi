namespace PedidosApi.DTOS.DeliveryDtos
{
    public class CreateDeliveryDto
    {
        public int OrderId { get; set; }

        public int? DeliveryPersonId { get; set; }

        public DateTime? EstimatedDelivery { get; set; }

        public DateTime? ActualDelivery { get; set; }

        public string DeliveryAddress { get; set; } = null!;

        public string? CurrentStatus { get; set; }
    }
}