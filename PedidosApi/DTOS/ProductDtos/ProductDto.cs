namespace PedidosApi.DTOS.ProductDtos
{
    public class ProductDto
    {
            public int Id { get; set; }

            public string Name { get; set; } = null!;

            public decimal Price { get; set; }

            public int Stock { get; set; }

            public string? Description { get; set; }

            public bool? Active { get; set; }
    }
}