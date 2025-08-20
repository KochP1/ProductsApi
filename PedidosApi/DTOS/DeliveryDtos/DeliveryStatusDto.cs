namespace PedidosApi.DTOS.DeliveryDtos
{
    public class DeliveryStatusDto
    {
        public int Id { get; set; }

        public int DeliveryId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? StatusDate { get; set; }

        public string? Notes { get; set; }
        public DeliveryDto Delivery { get; set; } = null!;
    }
}