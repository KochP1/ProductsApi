using System;
using System.Collections.Generic;

namespace PedidosApi.Models;

public partial class DeliveryStatusHistory
{
    public int Id { get; set; }

    public int DeliveryId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? StatusDate { get; set; }

    public string? Notes { get; set; }

    public virtual Delivery Delivery { get; set; } = null!;
}
