using System;
using System.Collections.Generic;

namespace PedidosApi.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Role { get; set; }

    public string? Phone { get; set; }

    public bool? Available { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
