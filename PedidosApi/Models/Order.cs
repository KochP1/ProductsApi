using System;
using System.Collections.Generic;

namespace PedidosApi.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
