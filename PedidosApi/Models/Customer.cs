using System;
using System.Collections.Generic;

namespace PedidosApi.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Address { get; set; } = null!;

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
