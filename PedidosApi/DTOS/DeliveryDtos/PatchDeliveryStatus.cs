namespace PedidosApi.DTOS.DeliveryDtos
{
    public class PatchDeliveryStatusDto
    {

        public int? OrderId { get; set; }

        public int DeliveryId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? StatusDate { get; set; }

        public string? Notes { get; set; }
    }
}