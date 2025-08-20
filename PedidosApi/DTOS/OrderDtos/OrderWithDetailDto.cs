using PedidosApi.DTOS.OrderDetailDtos;

namespace PedidosApi.DTOS.OrderDtos;

public class OrderWithDetailDto : OrderDto
{
    public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
}
