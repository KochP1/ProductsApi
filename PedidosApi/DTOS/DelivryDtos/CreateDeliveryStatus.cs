namespace PedidosApi.DTOS.DeliveryDtos
{
    public class CreateDeliveryStatus
    {
        public int DeliveryId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? StatusDate { get; set; }

        public string? Notes { get; set; }
    }
}