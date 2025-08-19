using System;
using System.Collections.Generic;

namespace PedidosApi.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? Description { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
