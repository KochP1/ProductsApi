using System;
using System.Collections.Generic;

namespace PedidosApi.Models;

public partial class Delivery
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int? DeliveryPersonId { get; set; }

    public DateTime? EstimatedDelivery { get; set; }

    public DateTime? ActualDelivery { get; set; }

    public string DeliveryAddress { get; set; } = null!;

    public string? CurrentStatus { get; set; }

    public virtual Employee? DeliveryPerson { get; set; }

    public virtual ICollection<DeliveryStatusHistory> DeliveryStatusHistories { get; set; } = new List<DeliveryStatusHistory>();

    public virtual Order Order { get; set; } = null!;
}
