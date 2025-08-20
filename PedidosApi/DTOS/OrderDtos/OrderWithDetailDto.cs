using PedidosApi.DTOS.DeliveryDtos;
using PedidosApi.DTOS.OrderDetailDtos;

namespace PedidosApi.DTOS.OrderDtos;

public class OrderWithDetailDto : OrderDto
{
    public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    public virtual DeliveryDto Delivery { get; set; } = null!;
}
